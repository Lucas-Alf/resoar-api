namespace Domain.Models
{
    public class FileResultModel
    {
        public byte[]? FileContents { get; set; }
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
    }
}
