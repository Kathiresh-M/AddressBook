using Entities.Models;

namespace Contracts
{
    public interface IAssetRepository
    {
        void AddAsset(Assert assetData);
        void DeleteAsset(Assert assetData);
        Assert GetAssetByAddressBookId(Guid AddressBookId);
        Assert GetAssetByAssetId(Guid AssetId);
        void UpdateAsset(Assert assetData);
        void Save();
    }
}
