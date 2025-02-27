using MediatR;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Application.Abstractions.Services;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Commands;

public class CreateDocument(string fileName, Stream content) : IRequest<Guid>
{
    public string FileName { get; } = fileName;
    public Stream Content { get; } = content;
}

internal sealed class CreateDocumentHandler(IFileManager fileManager, IDocumentRepository documentRepository)
    : IRequestHandler<CreateDocument, Guid>
{
    public async Task<Guid> Handle(CreateDocument request, CancellationToken cancellationToken)
    {
        var url = await fileManager.SaveDocumentAsync(request.FileName, request.Content, cancellationToken);
        var document = new Document(Guid.NewGuid(), url);
        documentRepository.Add(document);
        await documentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return document.Id;
    }
}