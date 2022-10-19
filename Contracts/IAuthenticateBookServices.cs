using Entities.Dto;
using Entities.Models;
using Services.Response;

namespace Contracts
{
    public interface IAuthenticateBookServices
    {
        TokenResponse AuthUser(LoginDto userData);
        bool Save();
    }
}