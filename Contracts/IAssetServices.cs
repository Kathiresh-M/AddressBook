using Entities.Models;

namespace Contracts
{
    public interface IAssetServices
    {
        AssetResponsetoUser AddAsset(Guid Id, Assert assetData, IFormFile file);
        AssetResponsetoUser GetAsset(Guid assetId);
    }
}
