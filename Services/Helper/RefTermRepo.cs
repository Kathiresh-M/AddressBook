using Contracts.Response;
using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class RefTermRepo : IRefTermRepo
    {
        private readonly BookRepository _context;

        public RefTermRepo(BookRepository context)
        {
            _context = context;
        }
        public RefTerm GetRefTerm(Guid refTermId)
        {
            if (refTermId == null || refTermId == Guid.Empty)
                throw new ArgumentNullException(nameof(refTermId) + " was null in GetRefTerm from RefTermRepository.");

            return _context.RefTerm.SingleOrDefault(refTerm => refTerm.Id == refTermId);
        }

        public RefTerm GetRefTerm(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key) + " was null in GetRefTerm from RefTermRepository.");

            return _context.RefTerm.FirstOrDefault(refTerm => refTerm.RefTerm_Key.ToLower() == key.ToLower());
        }
    }
}
