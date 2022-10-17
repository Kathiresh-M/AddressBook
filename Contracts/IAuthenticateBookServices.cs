using Entities.Dto;
using Entities.Models;

namespace Contracts
{
    public interface IAuthenticateBookServices
    {
        TokenResponse AuthUser(LoginDto userData);
        void AddBand(Profiles profile);
        bool Save();
    }
}