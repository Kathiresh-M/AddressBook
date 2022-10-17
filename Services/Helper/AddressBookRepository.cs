using AutoMapper;
using Contracts;
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

        public void UpdateAddressBook(Profiles addressBook, IEnumerable<Email> Emails, IEnumerable<Address> Addresses, IEnumerable<Phone> Phones)
        {

            _context.Profile.Update(addressBook);

            _context.Email.UpdateRange(Emails);

            _context.Address.UpdateRange(Addresses);

            _context.Phone.UpdateRange(Phones);

        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
