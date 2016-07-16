namespace Paynter.Core.Facebook.Models
{
    public class FacebookWebhookMessaging
    {
        public FacebookWebhookMessageUser Sender { get; set; }
        public FacebookWebhookMessageUser Recipient { get; set; }
        // public DateTime Timestamp { get; set; }
        public FacebookWebhookMessage Message { get; set; }
    }
}