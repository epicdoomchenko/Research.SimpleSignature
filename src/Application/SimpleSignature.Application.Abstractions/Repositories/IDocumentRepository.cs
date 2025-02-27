using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Abstractions.Repositories;

public interface IDocumentRepository : IBaseRepository<Document>
{
    Task<Document> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
}