using Contracts;
using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class RefSetRepo : IRefSetRepo
    {
        private readonly BookRepository _context;

        public RefSetRepo(BookRepository context)
        {
            _context = context;
        }
        public RefSet GetRefSet(Guid refSetId)
        {
            if (refSetId == null || refSetId == Guid.Empty)
                throw new ArgumentNullException(nameof(refSetId) + " was null in GetRefSet from RefSetRepository.");

            return _context.RefSet.SingleOrDefault(refSet => refSet.Id == refSetId);
        }
    }
}
