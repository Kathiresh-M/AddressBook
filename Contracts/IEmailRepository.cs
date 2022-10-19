using Entities.Models;

namespace Contracts
{
    public interface IEmailRepository
    {
        ICollection<Email> GetEmailsByAddressBookId(Guid addressBookId);
    }
}
