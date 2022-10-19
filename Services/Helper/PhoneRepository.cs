using Contracts;
using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly BookRepository _context;

        public PhoneRepository(BookRepository context)
        {
            _context = context;
        }
        public ICollection<Phone> GetPhonesByAddressBookId(Guid addressBookId)
        {
            var phones = _context.Phone.ToList();
            return phones.FindAll(phone => phone.Id == addressBookId);
        }
    }
}
