namespace Services.Response
{
    public class MessageResponsetoUser
    {
        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; }

        public MessageResponsetoUser(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
