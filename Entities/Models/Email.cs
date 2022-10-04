using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Email
    {
        [Key]
        public Guid Id { get; set; }

        [EmailAddress]
        public string User_Email { get; set; }

        [ForeignKey("Profile_Id")]
        public Profiles Profile { get; set; }
        public Guid Profile_Id { get; set; }

        [ForeignKey("RefTerm_Id")]
        public RefTerm RefTerm { get; set; }
        public Guid RefTerm_Id { get; set; }
    }
}
