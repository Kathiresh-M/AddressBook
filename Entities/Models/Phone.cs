using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    /// <summary>
    /// Used to get Phone details from user and map to Profile.
    /// </summary>
    public class Phone
    {
        [Key]
        public Guid Id { get; set; }

        [Phone]
        public string Phone_Number { get; set; }
        //public string Phone_type { get; set; }

        [ForeignKey("RefSetMappingId")]
        public Guid PhoneTypeId { get; set; }

        [ForeignKey("Profile_Id")]
        public Profiles Profile { get; set; }
        public Guid Profile_Id { get; set; }

        [ForeignKey("RefTerm_Id")]
        public RefTerm RefTerm { get; set; }
        public Guid RefTerm_Id { get; set; }
    }
}
