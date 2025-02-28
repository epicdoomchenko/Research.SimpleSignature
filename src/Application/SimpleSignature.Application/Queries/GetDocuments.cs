using MediatR;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Repositories;
using Document = SimpleSignature.Domain.Entities.Document;

namespace SimpleSignature.Application.Queries;

public class GetDocuments : IRequest<IReadOnlyCollection<DocumentData>>
{
    public long UserId { get; }

    public GetDocuments(long userId = -1)
    {
        UserId = userId;
    }
}

public class GetDocumentsHandler(IDocumentRepository documentRepository) : IRequestHandler<GetDocuments, IReadOnlyCollection<DocumentData>>
{
    public async Task<IReadOnlyCollection<DocumentData>> Handle(GetDocuments request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Document> documents;
        if (request.UserId == -1)
        {
            documents = await documentRepository.GetAllAsync(cancellationToken);
        }
        else
        {
            documents = await documentRepository.GetExcludingUserAsync(request.UserId, cancellationToken);
        }
        
        return documents.Select(d => new DocumentData(d)).ToList().AsReadOnly();
    }
}