using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class RefSet
    {
        [Key]
        public Guid Id { get; set; }
        public string RefSet_Key { get; set; }
    }
}
