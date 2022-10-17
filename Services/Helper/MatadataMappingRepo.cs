using Entities.Models;
using Repository;

namespace Services.Helper
{
    public class MatadataMappingRepo : IMetaDataMappingRepo
    {
        private readonly BookRepository _context;

        public MatadataMappingRepo(BookRepository context)
        {
            _context = context;
        }
        public RefSetTerm GetRefSetMapping(Guid refSetTermId)
        {
            if (refSetTermId == null || refSetTermId == Guid.Empty)
                throw new ArgumentNullException(nameof(refSetTermId) + " was null in GetRefSetMapping");

            return _context.RefSetTerm.SingleOrDefault(refSetTerm => refSetTerm.Id == refSetTermId);
        }

        public IEnumerable<RefTerm> GetRefTermsByRefSetId(Guid refSetId)
        {
            if (refSetId == null || refSetId == Guid.Empty)
                throw new ArgumentNullException(nameof(refSetId) + " was null in GetRefTermsByRefSetId from RefSetMappingRepository.");

            var refTerms = from refSet in _context.RefSet
                           join refSetterm in _context.RefSetTerm
                           on refSet.Id equals refSetterm.RefSetId
                           join refTerm in _context.RefTerm
                           on refSetterm.RefTermId equals refTerm.Id
                           select new
                           {
                               Id = refTerm.Id,
                               Key = refTerm.Key,
                               Description = refTerm.Description,
                               RefSetId = refSet.Id,
                           };

            var terms = new List<RefTerm>();

            foreach (var refTerm in refTerms)
            {
                if (refSetId == refTerm.RefSetId)
                    terms.Add(new RefTerm { Id = refTerm.Id, RefTerm_Key = refTerm.RefTerm_Key, Description = refTerm.Description });
            }

            return terms;
        }

        public IEnumerable<RefSetTerm> GetRefTermMappingId(Guid refSetId)
        {
            if (refSetId == null || refSetId == Guid.Empty)
                throw new ArgumentNullException(nameof(refSetId) + " was null in GetRefTermsByRefSetId from RefSetMappingRepository.");

            var refSetmappings = from refSet in _context.RefSet
                                 join refSetTerm in _context.RefSetTerm
                                 on refSet.Id equals refSetTerm.Id
                                 join refTerm in _context.RefTerm
                                 on refSetTerm.Id equals refTerm.Id
                                 select new RefSetTerm
                                 {
                                     Id = refSetTerm.Id,
                                     Id = refSetTerm.Id,
                                     Id = refSetTerm.Id
                                 };

            var mappings = new List<RefSetTerm>();

            foreach (var refSetMapping in refSetmappings)
            {
                if (refSetId == refSetMapping.RefSetId)
                    mappings.Add(refSetMapping);
            }

            return mappings;
        }
    }
}
