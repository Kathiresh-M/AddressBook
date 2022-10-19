namespace Services.Response
{
    public class TokenResponse : MessageResponsetoUser
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public TokenResponse(bool isSuccess, string message, string token, string tokenType) : base(isSuccess, message)
        {
            AccessToken = token;
            TokenType = tokenType;
        }
    }
}
