using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    /// <summary>
    /// Used to get File Details from user and map to Profile.
    /// </summary>
    public class Assert
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string DownloadUrl { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public decimal Size { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Profile_Id")]
        public Profiles Profile { get; set; }
        public Guid Profile_Id { get; set; }
    }
}
