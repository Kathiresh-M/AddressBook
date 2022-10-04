using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Profiles
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string First_Name { get; set; }

        [Required]
        public string Last_Name { get; set; }

        public string User_Name { get; set; }

        public ICollection<Email> Email { get; set; } = new List<Email>();

        [Required]
        [MaxLength(8), MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<Address> Address { get; set; } = new List<Address>();

        public ICollection<Phone> Phone { get; set; } = new List<Phone>();
    }
}
