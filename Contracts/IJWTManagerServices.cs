using Entities.Dto;
using Entities.Models;

namespace Contracts
{
    public interface IJWTManagerServices
    {
        Tokens Authenticate(LoginDto logindto);
    }
}
