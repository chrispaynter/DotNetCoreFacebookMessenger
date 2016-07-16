using Newtonsoft.Json;
using System.Collections.Generic;

namespace Paynter.FacebookMessenger.Models.SendApi
{
    public class SendApiPayload
    {
        [JsonProperty("template_type")]
        public SendApiPayloadTemplateType TemplateType { get; set; }
        public IEnumerable<SendApiElement> Elements { get; set; }
    }
}