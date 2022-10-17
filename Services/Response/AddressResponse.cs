namespace Services.Response
{
    public class AddressResponse : MessageResponsetoUser
    {
        public IEnumerable<Address> Addresses { get; protected set; }

        public AddressResponse(bool isSuccess, string message, IEnumerable<Address> addresses) : base(isSuccess, message)
        {
            Addresses = addresses;
        }
    }
}
