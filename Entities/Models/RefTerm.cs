using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class RefTerm
    {
        [Key]
        public Guid Id { get; set; }
        public string RefTerm_Key { get; set; }
    }
}
