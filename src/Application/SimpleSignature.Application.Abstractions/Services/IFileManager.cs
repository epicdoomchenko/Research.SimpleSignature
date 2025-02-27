namespace SimpleSignature.Application.Abstractions.Services;

public interface IFileManager
{
    Task<Uri> SaveDocumentAsync(string fileName, Stream document, CancellationToken cancellationToken = default);

    Task<byte[]> GetDocumentAsync(Uri fileUrl, CancellationToken cancellationToken = default);
}