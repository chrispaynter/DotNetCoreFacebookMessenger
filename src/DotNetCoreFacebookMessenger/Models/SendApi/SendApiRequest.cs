using Newtonsoft.Json;

namespace Paynter.FacebookMessenger.Models.SendApi
{
    public class SendApiRequest
    {
       public FacebookUser Recipient { get; set; }
       public SendApiMessage Message { get; set; }

       [JsonProperty("sender_action")]
       public string SenderAction { get; set; }
       
       [JsonProperty("notification_type")]
       public string NotificationType { get; set; }
    }
}