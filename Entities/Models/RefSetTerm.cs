using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class RefSetTerm
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("RefSet_Id")]
        public RefSet RefSet { get; set; }
        public Guid RefSet_Id { get; set; }

        [ForeignKey("RefTerm_Id")]
        public RefTerm RefTerm { get; set; }
        public Guid RefTerm_Id { get; set; }
    }
}
