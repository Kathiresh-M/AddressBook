using Contracts;
using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class EmailRepository : IEmailRepository
    {
        private readonly BookRepository _context;

        public EmailRepository(BookRepository context)
        {
            _context = context;
        }
        public ICollection<Email> GetEmailsByAddressBookId(Guid addressBookId)
        {
            var Emails = _context.Email.ToList();
            return Emails.FindAll(Email => Email.Id == addressBookId);
        }
    }
}
