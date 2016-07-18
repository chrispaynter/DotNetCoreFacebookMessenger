using System;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Paynter.FacebookMessenger.Models;
using Paynter.FacebookMessenger.Models.SendApi;
using Paynter.FacebookMessenger.Configuration;
using Paynter.FacebookMessenger.Models.Webhooks;
using System.Net;

namespace Paynter.FacebookMessenger.Services
{
    public delegate void MessageEventHandler(WebhookMessaging messageEvent);
    
    public class FacebookMessengerService
    {
        private FacebookOptions _options;
        private ILogger<FacebookMessengerService> _logger;
        public event MessageEventHandler MessageRecieved;
        private HttpClient _httpClient;

        public FacebookMessengerService(IOptions<FacebookOptions> options, ILogger<FacebookMessengerService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public HttpClient HttpClient 
        { 
            get
            {
                if(_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    _httpClient.BaseAddress = new Uri($"{_options.ApiUrl}?access_token={_options.AccessToken}");                    
                }

                return _httpClient;
            }
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
            var requestString = new StringContent(Serialise(messageData), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync("", requestString);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                var contents = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error sending message to Facebook API", contents, requestString);
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