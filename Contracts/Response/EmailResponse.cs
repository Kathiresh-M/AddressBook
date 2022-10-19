using Entities.Models;

namespace Services.Response
{
    public class EmailResponse : MessageResponsetoUser
    {
        public ICollection<Email> Email { get; protected set; }

        public EmailResponse(bool isSuccess, string message, ICollection<Email> email) : base(isSuccess, message)
        {
            Email = email;
        }
    }
}
