namespace Application.Interfaces.Services.Domain
{
    public interface IPdfDocumentService
    {
        string ExtractText(byte[] file);
        byte[] GenerateThumbnail(byte[] file);
    }
}
