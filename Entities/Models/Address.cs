using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State_Name { get; set; }
        public string Country { get; set; }

        public string ZipCode { get; set; }

        [ForeignKey("User_Id")]
        public Profiles Profile { get; set; }
        public Guid User_Id { get; set; }

        [ForeignKey("RefTerm_Id")]
        public RefTerm RefTerm { get; set; }
        public Guid RefTerm_Id { get; set; }
    }
}
