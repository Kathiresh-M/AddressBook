namespace Entities.Dto.ManipulationDto
{
    public abstract class EmailManipulationDto
    {
        public string User_Email { get; set; }
        public virtual string type { get; set; }
    }
}
