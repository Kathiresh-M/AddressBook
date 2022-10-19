using Contracts;
using Entities.Models;
using Services.Response;

namespace Services
{
    public class AssetServices : IAssetServices
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAddressBookRepository _addressBookRepository;

        public AssetServices(IAssetRepository assetRepository,IAddressBookRepository addressBookRepository)
        {
            _assetRepository = assetRepository;
            _addressBookRepository = addressBookRepository;
        }

        /// <summary>
        /// Method to Create asset
        /// </summary>
        /// <param name="Id">Address Book Id</param>
        /// <param name="tokenUserId">Token Id</param>
        /// <param name="assetData">Asset data for create</param>
        /// <param name="file">Asset file data</param>
        public AssetResponsetoUser AddAsset(Guid Id, Guid tokenUserId, 
            Assert assetData, IFormFile file)
        {
            var addressBook = _addressBookRepository.GetAddressBookById(Id);
            var assetExists = _assetRepository.GetAssetByAddressBookId(Id);

            if (assetExists != null && assetExists.Id.Equals(tokenUserId))
                return new AssetResponsetoUser(false, "Asset already exists", null);

            if (assetExists != null && !assetExists.Id.Equals(tokenUserId))
                return new AssetResponsetoUser(false, "AddressBook not found", null);

            assetData.Id = Id;
            assetData.FileName = file.FileName;
            assetData.Size = file.Length;
            assetData.FileType = file.ContentType;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                assetData.Content = Convert.ToBase64String(ms.ToArray());
            }

            _assetRepository.AddAsset(assetData);
            _assetRepository.Save();

            return new AssetResponsetoUser(true, null, assetData);
        }

        /// <summary>
        /// Method to Get Asset
        /// </summary>
        /// <param name="assetId">Id of asset</param>
        /// <param name="tokenUserId">Token Id</param>
        public AssetResponsetoUser GetAsset(Guid assetId, Guid tokenUserId)
        {
            var asset = _assetRepository.GetAssetByAssetId(assetId);

            if (asset == null)
                return new AssetResponsetoUser(false, "Asset not found", null);

            if (!asset.Id.Equals(tokenUserId))
                return new AssetResponsetoUser(false, "Asset not found", null);

            return new AssetResponsetoUser(true, null, asset);
        }
    }
}
