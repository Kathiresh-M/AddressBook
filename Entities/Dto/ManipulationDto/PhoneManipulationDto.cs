namespace Entities.Dto.ManipulationDto
{
    public abstract class PhoneManipulationDto
    {
        public int Phone_Number { get; set; }
        public virtual string Phone_type { get; set; }
    }
}
