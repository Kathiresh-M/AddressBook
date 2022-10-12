using Entities.Models;

namespace Contracts
{
    public interface IAddressBookRepository
    {
        Profiles GetAddressBookById(Guid AddressBookId);
    }
}
