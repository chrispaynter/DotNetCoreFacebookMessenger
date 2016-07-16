using System.Collections.Generic;

namespace Paynter.Core.Facebook.Models
{
    public class FacebookWebhookRequest
    {
        public FacebookWebhookObject Object { get; set; }
        public IEnumerable<FacebookWebhookEntry> Entry { get; set; }
    }
}