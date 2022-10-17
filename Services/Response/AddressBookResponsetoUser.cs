using Entities.Dto;

namespace Services.Response
{
    public class AddressBookResponsetoUser : MessageResponsetoUser
    {
        public AddressBookDto AddressBook { get; protected set; }
        public AddressBookResponsetoUser(bool isSuccess, string message, AddressBookDto addressBook) : base(isSuccess, message)
        {
            AddressBook = addressBook;
        }
    }
}
