namespace Paynter.FacebookMessenger.Models.SendApi
{
    public class SendApiMessage
    {
        public string Text { get; set; }
        public SendApiAttachment Attachment { get; set; }
        public string MetaData { get; set; }
    }
}