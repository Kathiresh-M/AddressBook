using Entities.Dto;

namespace Services.Response
{
    public class AuthResponse : MessageResponsetoUser
    {
        public LoginDto user { get; protected set; }
        public AuthResponse(bool isSuccess, string message, LoginDto user) : base(isSuccess, message)
        {
            this.user = user;
        }
    }
}
