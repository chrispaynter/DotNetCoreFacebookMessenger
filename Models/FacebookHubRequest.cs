namespace Paynter.Core.Facebook.Models
{
    public class FacebookHubRequest
    {
        public string Mode { get; set; }
        public string Challenge { get; set; }

        public string Verify_Token { get; set; }
    }
}