namespace Paynter.FacebookMessenger.Models
{
    public class FacebookUser
    {
        public FacebookUser(){}
        public FacebookUser(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}