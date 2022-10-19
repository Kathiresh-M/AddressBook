using Entities.Models;

namespace Entities.Dto
{
    public class AddressBookDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Email> Emails { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }
}
