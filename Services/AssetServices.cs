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

        public AssetResponsetoUser AddAsset(Guid Id, Assert assetData, IFormFile file)
        {
            var addressBook = _addressBookRepository.GetAddressBookById(Id);
            var assetExists = _assetRepository.GetAssetByAddressBookId(Id);
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
        public AssetResponsetoUser GetAsset(Guid assetId)
        {
            var asset = _assetRepository.GetAssetByAssetId(assetId);
            return new AssetResponsetoUser(true, null, asset);
        }
    }
}
