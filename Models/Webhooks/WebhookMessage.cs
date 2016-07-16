namespace Paynter.FacebookMessenger.Models.Webhooks
{
    public class WebhookMessage
    {
        public string Mid { get; set; }
        public int Seq { get; set; }
        public string Text { get; set; }
    }
}