using Microsoft.EntityFrameworkCore;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL.Repositories;

internal class DocumentRepository : BaseRepository<Document>, IDocumentRepository
{
    public DocumentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Document> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        return DbSet.SingleAsync(d => d.Id == documentId, cancellationToken);
    }
}