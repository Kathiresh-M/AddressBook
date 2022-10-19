using Entities.Models;

namespace Services.Response
{
    public class AddressResponse : MessageResponsetoUser
    {
        public ICollection<Address> Address { get; protected set; }

        public AddressResponse(bool isSuccess, string message, ICollection<Address> address) : base(isSuccess, message)
        {
            Address = address;
        }
    }
}
