namespace Paynter.Core.Facebook.Models
{
    public class FacebookWebhookMessage
    {
        public string Mid { get; set; }
        public int Seq { get; set; }
        public string Text { get; set; }
    }
}