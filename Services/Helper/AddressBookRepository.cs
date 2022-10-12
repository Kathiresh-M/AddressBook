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
    }
}
