using Contracts;
using Entities.Models;

namespace Services.Helper
{
    public class EmailRepository : IEmailRepository
    {
        private readonly BookRepostiry _context;

        public EmailRepository(BookRepostiry context)
        {
            _context = context;
        }
        public IEnumerable<Email> GetEmailsByAddressBookId(Guid addressBookId)
        {
            var Emails = _context.Email.ToList();
            return Emails.FindAll(Email => Email.AddressBookId == addressBookId);
        }
    }
}
