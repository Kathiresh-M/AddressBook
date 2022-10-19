namespace Services.Helper
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }

        public Token(string accessToken, string tokenType)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
        }
    }
}
