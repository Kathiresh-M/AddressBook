using Entities.Dto.ManipulationDto;

namespace Entities.Dto
{
    public class PhoneDto : PhoneManipulationDto
    {

    }

    public class PhoneUpdationDTO : PhoneDto
    {
        public Guid Id { get; set; }
    }
}
