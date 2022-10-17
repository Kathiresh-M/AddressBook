namespace Services.Response
{
    public class EmailResponse : MessageResponsetoUser
    {
        public IEnumerable<Email> Emails { get; protected set; }

        public EmailResponse(bool isSuccess, string message, IEnumerable<Email> emails) : base(isSuccess, message)
        {
            Emails = emails;
        }
    }
}
