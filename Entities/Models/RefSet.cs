using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// Used to get RefSet key .
    /// </summary>
    public class RefSet
    {
        [Key]
        public Guid Id { get; set; }
        public string RefSet_Key { get; set; }
    }
}
