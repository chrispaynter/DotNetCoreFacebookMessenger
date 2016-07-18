using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Paynter.FacebookMessenger.Converters;

namespace Paynter.FacebookMessenger.Models.Webhooks
{
    public class WebhookEntry
    {
        public string Id { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime Time { get; set; }
        public IEnumerable<WebhookMessaging> Messaging { get; set; }
    }
}