namespace Services.Response
{
    public class PhoneResponse: MessageResponsetoUser
    {
        public IEnumerable<Phone> Phones { get; protected set; }

        public PhoneResponse(bool isSuccess, string message, IEnumerable<Phone> phones) : base(isSuccess, message)
        {
            Phones = phones;
        }
    }
}
