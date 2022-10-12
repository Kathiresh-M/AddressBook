namespace Entities.Dto
{
    public class MessagetoUser
    {
        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; }

        public MessagetoUser(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
