using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// Used to get RefTerm Key for RefSet Key.
    /// </summary>
    public class RefTerm
    {
        [Key]
        public Guid Id { get; set; }
        public string RefTerm_Key { get; set; }
        public string Description { get; set; }
    }
}
