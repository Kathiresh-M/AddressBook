using Entities.Models;

namespace Contracts
{
    public interface IMetaDataMappingRepo
    {
        RefSetTerm GetRefSetMapping(Guid refSetTermId);
        IEnumerable<RefTerm> GetRefTermsByRefSetId(Guid refSetId);
        IEnumerable<RefSetTerm> GetRefTermMappingId(Guid refSetId);
    }
}
