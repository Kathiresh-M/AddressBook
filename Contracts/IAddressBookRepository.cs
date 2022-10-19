using Entities.Dto;
using Entities.Models;

namespace Contracts
{
    public interface IAddressBookRepository
    {
        Profiles GetAddressBookById(Guid AddressBookId);
        Profiles GetAddressBookByName(string firstName, string lastName);
        void DeleteAddressBook(Profiles addressBook);
        void CreateAddressBook(AddressBookDto addressBookData);
        bool Save();
        void UpdateAddressBook(Profiles addressBook, ICollection<Email> Emails, ICollection<Address> Addresses, ICollection<Phone> Phones);
    }
}
