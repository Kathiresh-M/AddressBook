using Entities.Models;

namespace Contracts
{
    public interface IPhoneRepository
    {
        ICollection<Phone> GetPhonesByAddressBookId(Guid addressBookId);
    }
}
