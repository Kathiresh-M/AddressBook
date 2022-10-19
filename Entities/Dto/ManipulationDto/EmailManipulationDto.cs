using Entities.Models;

namespace Entities.Dto.ManipulationDto
{
    public abstract class EmailManipulationDto
    {
        public string User_Email { get; set; }
        public virtual TypeReference type { get; set; }
    }
}
