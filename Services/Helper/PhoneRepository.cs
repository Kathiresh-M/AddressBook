using Contracts;
using Entities.Models;

namespace Services.Helper
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly BookRepostiry _context;

        public PhoneRepository(BookRepostiry context)
        {
            _context = context;
        }
        public ICollection<Phone> GetPhonesByAddressBookId(Guid addressBookId)
        {
            var phones = _context.Phone.ToList();
            return phones.FindAll(phone => phone.AddressBookId == addressBookId);
        }
    }
}
