using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Repository;

namespace Services.Repository
{
    public class AddressBookRepository : IAddressBookRepository
    {
        private readonly BookRepository _context;

        public AddressBookRepository(BookRepository context)
        {
            _context = context;
        }
        public Profiles GetAddressBookById(Guid AddressBookId)
        {
            return _context.Profile.SingleOrDefault(addressBook => addressBook.Id == AddressBookId);
        }

        public void CreateAddressBook(AddressBookDto addressBookData)
        {
            var addressBook = new Profiles
            {
                Id = addressBookData.Id,
                First_Name = addressBookData.FirstName,
                Last_Name = addressBookData.LastName,
            };

            _context.Profile.Add(addressBook);

            _context.Email.AddRange(addressBookData.Emails);

            _context.Address.AddRange(addressBookData.Addresses);

            _context.Phone.AddRange(addressBookData.Phones);

        }

        public void UpdateAddressBook(Profiles addressBook, ICollection<Email> Emails, ICollection<Address> Addresses, ICollection<Phone> Phones)
        {

            _context.Profile.Update(addressBook);

            _context.Email.UpdateRange(Emails);

            _context.Address.UpdateRange(Addresses);

            _context.Phone.UpdateRange(Phones);

        }

        public Profiles GetAddressBookByName(string firstName, string lastName)
        {
            return _context.Profile.SingleOrDefault(addressBook => addressBook.First_Name == firstName && addressBook.Last_Name == lastName);
        }

        public void DeleteAddressBook(Profiles addressBook)
        {
            _context.Profile.Remove(addressBook);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
