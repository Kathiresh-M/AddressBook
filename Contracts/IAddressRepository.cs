using Entities.Models;

namespace Contracts
{
    public interface IAddressRepository
    {
        ICollection<Address> GetAddresssByAddressBookId(Guid addressBookId);
        
    }
}
