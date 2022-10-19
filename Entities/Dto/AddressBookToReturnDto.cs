namespace Entities.Dto
{
    public class AddressBookToReturnDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<EmailToReturnDto> Emails { get; set; }
        public ICollection<AddressToReturnDTO> Addresses { get; set; }
        public ICollection<PhoneToReturnDTO> Phones { get; set; }
    }
}
