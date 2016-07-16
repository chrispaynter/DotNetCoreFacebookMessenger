using System.Collections.Generic;

namespace Paynter.FacebookMessenger.Models.Webhooks
{
    public class WebhookEntry
    {
        public string Id { get; set; }
        // public DateTime? Time { get; set; }
        public IEnumerable<WebhookMessaging> Messaging { get; set; }
    }
}