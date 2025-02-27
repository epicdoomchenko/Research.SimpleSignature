namespace SimpleSignature.Application.Abstractions;

public interface IFileManager
{
    Task<Uri> SaveDocumentAsync(string fileName, byte[] document, CancellationToken cancellationToken = default);

    Task<byte[]> GetDocumentAsync(Uri fileUrl, CancellationToken cancellationToken = default);
}