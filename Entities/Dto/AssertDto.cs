namespace Entities.Dto
{
    public class AssertDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string DownloadURL { get; set; }
        public string FileType { get; set; }
        public int Size { get; set; }
        public string FileContent { get; set; }
    }
}
