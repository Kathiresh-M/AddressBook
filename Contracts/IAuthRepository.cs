using Entities.Models;

namespace Contracts
{
    public interface IAuthRepository
    {
        Profiles GetUser(string userName);
        Profiles GetUser(Guid userId);
    }
}
