using Entities.Models;

namespace Services.Response
{
    public class PhoneResponse: MessageResponsetoUser
    {
        public ICollection<Phone> Phone { get; protected set; }

        public PhoneResponse(bool isSuccess, string message, ICollection<Phone> phone) : base(isSuccess, message)
        {
            Phone = phone;
        }
    }
}
