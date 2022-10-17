using Entities.Dto;
using Entities.Models;

namespace Contracts
{
    public interface IJWTManagerServices
    {
        string GenerateSecurityToken(LoginDto login);
        int? ValidateJwtToken(string token);
        //Tokens Authenticate(LoginDto logindto);
    }
}
