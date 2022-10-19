using Entities.Models;

namespace Contracts
{
    public interface IRefSetRepo
    {
        RefSet GetRefSet(Guid refSetId);
        RefSet GetRefSet(string set);
    }
}
