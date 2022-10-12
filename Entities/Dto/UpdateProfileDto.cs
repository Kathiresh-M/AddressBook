namespace Entities.Dto
{
    public class UpdateProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<EmailUpdationDTO> Emails { get; set; }
        public ICollection<AddressUpdationDTO> Addresses { get; set; }
        public ICollection<PhoneUpdationDTO> Phones { get; set; }
    }
}
