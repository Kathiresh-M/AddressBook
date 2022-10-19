using Entities.Dto;
using Entities.Models;

namespace Contracts
{
    public interface IJWTManagerServices
    {
        string GenerateSecurityToken(Profiles login);
        int? ValidateJwtToken(string token);
        //Tokens Authenticate(LoginDto logindto);
    }
}
