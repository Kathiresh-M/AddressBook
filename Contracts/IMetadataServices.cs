using Services.Response;

namespace Contracts
{
    public interface IMetadataServices
    {
        RefTermResponse GetRefTermsByRefSetId(Guid Id);
    }
}
