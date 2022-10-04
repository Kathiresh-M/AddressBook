namespace Entities.Dto
{
    public class ProfileforCreatingDto
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public ICollection<EmailDto> Email { get; set; } = new List<EmailDto>();
        public ICollection<AddressDto> Address { get; set; } = new List<AddressDto>();
        public ICollection<PhoneDto> Phone { get; set; } = new List<PhoneDto>();
    }
}
