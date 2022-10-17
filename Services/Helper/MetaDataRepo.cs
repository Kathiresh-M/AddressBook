using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class MetaDataRepo
    {
        private readonly BookRepository _context;

        public MetaDataRepo(BookRepository bookRepository)
        {
            _context = bookRepository;
        }
        public RefSet GetRefSet(Guid refSetId)
        {
            if (refSetId == null || refSetId == Guid.Empty)
                throw new ArgumentNullException(nameof(refSetId) + " was null in GetRefSet from RefSetRepository.");

            return _context.RefSet.SingleOrDefault(refSet => refSet.Id == refSetId);
        }
    }
}
