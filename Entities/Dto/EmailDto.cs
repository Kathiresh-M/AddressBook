using Entities.Dto.ManipulationDto;

namespace Entities.Dto
{
    public class EmailDto : EmailManipulationDto
    {

    }

     public class EmailUpdationDto : EmailDto
    {
        public Guid Id { get; set; }
    }
    public class EmailToReturnDto : EmailUpdationDto { }
}
