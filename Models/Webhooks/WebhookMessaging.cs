namespace Paynter.FacebookMessenger.Models.Webhooks
{
    public class WebhookMessaging
    {
        public FacebookUser Sender { get; set; }
        public FacebookUser Recipient { get; set; }
        // public DateTime Timestamp { get; set; }
        public WebhookMessage Message { get; set; }
    }
}