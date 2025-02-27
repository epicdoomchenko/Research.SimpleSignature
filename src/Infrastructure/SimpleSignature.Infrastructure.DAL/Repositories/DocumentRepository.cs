using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL.Repositories;

internal class DocumentRepository : BaseRepository<Document>, IDocumentRepository
{
    public DocumentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}