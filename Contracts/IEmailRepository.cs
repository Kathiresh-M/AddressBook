using Entities.Models;

namespace Contracts
{
    public interface IEmailRepository
    {
        IEnumerable<Email> GetEmailsByAddressBookId(Guid addressBookId);
    }
}
