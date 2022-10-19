using Entities.Dto;

namespace Services.Helper
{
    public class AddressBookUpdate
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public ICollection<EmailUpdationDto> Email { get; set; }
        public ICollection<AddressUpdationDTO> Address { get; set; }
        public ICollection<PhoneUpdationDTO> Phone { get; set; }
        public AssertIdDto Asset { get; set; }
    }
}
