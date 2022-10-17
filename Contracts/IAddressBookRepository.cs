using Entities.Models;

namespace Contracts
{
    public interface IAddressBookRepository
    {
        Profiles GetAddressBookById(Guid AddressBookId);
        bool Save();
        void UpdateAddressBook(Profiles addressBook, IEnumerable<Email> Emails, IEnumerable<Address> Addresses, IEnumerable<Phone> Phones);
    }
}
