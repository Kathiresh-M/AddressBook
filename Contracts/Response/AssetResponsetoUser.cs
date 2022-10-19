using Entities.Models;

namespace Services.Response
{
    public class AssetResponsetoUser : MessageResponsetoUser
    {
        public Assert Asset { get; protected set; }

        public AssetResponsetoUser(bool isSuccess, string message, Assert assetData) : base(isSuccess, message)
        {
            Asset = assetData;
        }
    }
}
