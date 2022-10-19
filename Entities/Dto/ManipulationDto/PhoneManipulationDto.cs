
using Entities.Models;

namespace Entities.Dto.ManipulationDto
{
    public abstract class PhoneManipulationDto
    {
        public string Phone_Number { get; set; }
        public virtual TypeReference Phone_type { get; set; }
    }
}
