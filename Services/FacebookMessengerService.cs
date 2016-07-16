using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Paynter.FacebookMessenger.Models;
using Paynter.FacebookMessenger.Models.SendApi;
using Paynter.FacebookMessenger.Configuration;
using Paynter.FacebookMessenger.Models.Webhooks;

namespace Paynter.FacebookMessenger.Services
{
    public delegate void MessageEventHandler(WebhookMessaging messageEvent);
    
    public class FacebookMessengerService
    {
        private FacebookOptions _options;
        private ILogger<FacebookMessengerService> _logger;
        private string _apiUrl;
        public event MessageEventHandler MessageRecieved;

        public FacebookMessengerService(IOptions<FacebookOptions> options, ILogger<FacebookMessengerService> logger)
        {
            _options = options.Value;
            _logger = logger;
            _apiUrl = $"{_options.ApiUrl}/v{_options.ApiVersion}/me";
        }

        public bool ValidateHubRequest(WebhookHubRequest request)
        {
            if (request.Mode == "subscribe")
            {
                return request.Verify_Token.Equals(_options.HubChallenge);
            }

            return false;
        }


        public void ProcessWebhookRequest(WebhookCallback request)
        {
            if (request.Object == WebhookObject.Page)
            {
                foreach (var entry in request.Entry)
                {
                    foreach (var messagingEvent in entry.Messaging)
                    {
                        if (messagingEvent.Message != null)
                        {
                            if(MessageRecieved != null) MessageRecieved(messagingEvent);
                        }
                        else
                        {
                            _logger.LogWarning("Webhook recieved unknown messaging event", request);
                        }
                    }
                }
            }
        }

        public void SendMarkSeen(string recipientId)
        {
            SendSenderAction(recipientId, "mark_seen");
        }

        public void SendTypingOn(string recipientId)
        {
            SendSenderAction(recipientId, "typing_on");
        }

        public void SendTypingOff(string recipientId)
        {
            SendSenderAction(recipientId, "typing_off");
        }

        public void SendTextMessage(string recipientId, string messageText)
        {
            var messageRequest = new SendApiRequest()
            {
                Recipient = new FacebookUser(recipientId),
                Message = new SendApiMessage()
                {
                    Text = messageText
                }
            };

            CallSendApi(messageRequest);
        }

        private void SendSenderAction(string recipientId, string action)
        {
            var request = new SendApiRequest()
            {
                Recipient = new FacebookUser() {
                    Id = recipientId
                },
                SenderAction = action
            };

            CallSendApi(request);
        }

        public async void CallSendApi(object messageData)
        {
            var uri = $"{_apiUrl}/messages?access_token={_options.AccessToken}";

            var request = WebRequest.Create(uri);

            ASCIIEncoding encoding = new ASCIIEncoding();
            var jsonData = Serialise(messageData);
            byte[] data = encoding.GetBytes(jsonData);
            request.Method = "Post";
            request.ContentType = "application/json";

            var stream = await request.GetRequestStreamAsync();
            stream.Write(data, 0, data.Length);
            stream.Dispose();

            try
            {
                var responseRaw = await request.GetResponseAsync();
                var response = responseRaw as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var responseText = reader.ReadToEnd();
                    response.Dispose();
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError("Error sending message to Facebook API", e, messageData);
            }
        }

        private static string Serialise(object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}