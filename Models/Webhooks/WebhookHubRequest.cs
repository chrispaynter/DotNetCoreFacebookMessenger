namespace Paynter.FacebookMessenger.Models.Webhooks
{
    public class WebhookHubRequest
    {
        public string Mode { get; set; }
        public string Challenge { get; set; }

        public string Verify_Token { get; set; }
    }
}