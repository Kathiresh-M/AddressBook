using Entities.Dto.ManipulationDto;

namespace Entities.Dto
{
    public class AddressDto : AddressManipulationDto
    {

    }

    public class AddressUpdationDTO : AddressDto
    {
        public Guid Id { get; set; }
    }

    public class AddressToReturnDTO : AddressUpdationDTO { }
}
