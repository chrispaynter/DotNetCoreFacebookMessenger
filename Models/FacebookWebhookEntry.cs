using System.Collections.Generic;

namespace Paynter.Core.Facebook.Models
{
    public class FacebookWebhookEntry
    {
        public string Id { get; set; }
        // public DateTime? Time { get; set; }
        public IEnumerable<FacebookWebhookMessaging> Messaging { get; set; }
    }
}