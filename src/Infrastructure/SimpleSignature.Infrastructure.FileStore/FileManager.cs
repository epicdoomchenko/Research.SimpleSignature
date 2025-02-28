using SimpleSignature.Application.Abstractions.Services;

namespace SimpleSignature.Infrastructure.FileStore;

internal class FileManager : IFileManager
{
    private static readonly string DocumentStorePath;

    static FileManager()
    {
        DocumentStorePath = Path.Combine(Directory.GetCurrentDirectory(), "DocumentStore");
        if (!Directory.Exists(DocumentStorePath))
        {
            Directory.CreateDirectory(DocumentStorePath);
        }
    }

    public async Task<Uri> SaveDocumentAsync(string fileName, Stream document,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        if (document == null || document.Length == 0)
        {
            throw new ArgumentException("Document cannot be null or empty.", nameof(document));
        }

        var filePath = Path.Combine(DocumentStorePath, fileName);

        using (Stream fileWriter = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            await document.CopyToAsync(fileWriter, cancellationToken);
        }
        return new Uri(filePath);
    }

    public async Task<byte[]> GetDocumentAsync(Uri fileUrl, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileUrl);

        var filePath = fileUrl.LocalPath;

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }

        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }
}