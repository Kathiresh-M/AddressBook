using Contracts;
using Services.Helper;
using Services.Response;

namespace Services
{
    public class MetadataServices : IMetadataServices
    {
        private readonly MetaDataRepo _metaDataRepo;
        private readonly IMetaDataMappingRepo _metaDataMappingRepo;

        public MetadataServices(MetaDataRepo metaDataRepo, IMetaDataMappingRepo metaDataMappingRepo)
        {
            _metaDataRepo = metaDataRepo;
            _metaDataMappingRepo = metaDataMappingRepo;
        }
        public RefTermResponse GetRefTermsByRefSetId(Guid Id)
        {

            var refSet = _metaDataRepo.GetRefSet(Id);

            if (refSet == null)
                return new RefTermResponse(false, "Refset does not exists", null);

            var refTerms = _metaDataMappingRepo.GetRefTermsByRefSetId(Id);

            int count = refTerms.Count();
            if (count == 0)
            {
                return new RefTermResponse(false, $"There are no RefTerms under ref set Id: {Id}", null);
            }

            return new RefTermResponse(true, null, refTerms);
        }
    }
}
