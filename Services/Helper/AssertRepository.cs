using Contracts;
using Entities.Models;
using Repository;

namespace Services.Repository
{
    public class AssertRepository : IAssetRepository
    {
        private readonly BookRepository _context;

        public AssertRepository(BookRepository context)
        {
            _context = context;
        }

        public void AddAsset(Assert assetData)
        {
            if (assetData == null)
                throw new ArgumentNullException(nameof(assetData) + " was null in AddAsset from AssetRepository");

            _context.Assert.Add(assetData);
        }

        public Assert GetAssetByAssetId(Guid AssetId)
        {
            if (AssetId == null || AssetId == Guid.Empty)
                throw new ArgumentNullException(nameof(AssetId) + " was null in GetAsset from AssetRepository");

            return _context.Assert.SingleOrDefault(asset => asset.Id == AssetId);
        }

        public Assert GetAssetByAddressBookId(Guid AddressBookId)
        {
            if (AddressBookId == null || AddressBookId == Guid.Empty)
                throw new ArgumentNullException(nameof(AddressBookId) + " was null in GetAsset from AssetRepository");

            return _context.Assert.SingleOrDefault(asset => asset.Id == AddressBookId);
        }

        public void UpdateAsset(Assert assetData)
        {
            if (assetData == null)
                throw new ArgumentNullException(nameof(assetData) + " was null in UpdateAsset from AssetRepository");

            _context.Assert.Update(assetData);
        }

        public void DeleteAsset(Assert assetData)
        {
            if (assetData == null)
                throw new ArgumentNullException(nameof(assetData) + " was null in DeleteAsset from AssetRepository");

            _context.Assert.Remove(assetData);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}