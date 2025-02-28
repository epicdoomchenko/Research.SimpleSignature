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

    public async Task<IReadOnlyCollection<Document>> GetExcludingUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        return (await DbSet
            .Where(c => c.UserDocuments.All(u => u.UserId != userId))
            .ToListAsync(cancellationToken)).AsReadOnly();
    }
}