using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Phone
    {
        [Key]
        public Guid Id { get; set; }

        [Phone]
        public int Phone_Number { get; set; }
        public string Phone_type { get; set; }

        [ForeignKey("Profile_Id")]
        public Profiles Profile { get; set; }
        public Guid Profile_Id { get; set; }

        [ForeignKey("RefTerm_Id")]
        public RefTerm RefTerm { get; set; }
        public Guid RefTerm_Id { get; set; }
    }
}
