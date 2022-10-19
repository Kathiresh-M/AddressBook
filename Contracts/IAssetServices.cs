using Entities.Models;
using Services.Response;

namespace Contracts
{
    public interface IAssetServices
    {
        AssetResponsetoUser AddAsset(Guid Id, Guid tokenUserId,
            Assert assetData, IFormFile file);
        AssetResponsetoUser GetAsset(Guid assetId, Guid tokenUserId);
    }
}
