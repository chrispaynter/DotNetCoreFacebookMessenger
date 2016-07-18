using System.Collections.Generic;

namespace Paynter.FacebookMessenger.Models.Webhooks
{
    public class WebhookCallback
    {
        public WebhookObject Object { get; set; }
        public IEnumerable<WebhookEntry> Entry { get; set; }
    }
}