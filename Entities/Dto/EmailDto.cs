using Entities.Dto.ManipulationDto;

namespace Entities.Dto
{
    public class EmailDto : EmailManipulationDto
    {

    }

     public class EmailUpdationDTO : EmailDto
    {
        public Guid Id { get; set; }
    }
}
