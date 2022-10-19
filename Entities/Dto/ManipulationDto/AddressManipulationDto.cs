using Entities.Models;

namespace Entities.Dto.ManipulationDto
{
    public abstract class AddressManipulationDto
    {
        public string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State_Name { get; set; }
        public virtual TypeReference type { get; set; }
        public virtual TypeReference Country { get; set; }
        public virtual string ZipCode { get; set; }
    }
}
