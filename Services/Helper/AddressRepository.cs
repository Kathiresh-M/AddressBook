using Contracts;
using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class AddressRepository : IAddressRepository
    {
        private readonly BookRepository _context;

        public AddressRepository(BookRepository context)
        {
            _context = context;
        }
        public ICollection<Address> GetAddresssByAddressBookId(Guid addressBookId)
        {
            var Addresss = _context.Address.ToList();
            return Addresss.FindAll(Address => Address.AddressBookId == addressBookId);
        }
    }
}
