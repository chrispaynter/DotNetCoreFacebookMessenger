using System.Collections.Generic;

namespace Paynter.FacebookMessenger.Models.SendApi
{
    public class SendApiElement
    {
        public string Title { get; set; }
        public string Image_Url { get; set; }
        public string Subtitle { get; set; }

        public IEnumerable<SendApiElementButton> Buttons { get; set; }
    }
}