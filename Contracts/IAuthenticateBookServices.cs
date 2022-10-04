using Entities.Models;

namespace Contracts
{
    public interface IAuthenticateBookServices
    {
        void AddBand(Profiles profile);
        bool Save();
    }
}
