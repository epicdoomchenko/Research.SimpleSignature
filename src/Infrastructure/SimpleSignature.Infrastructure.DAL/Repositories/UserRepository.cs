using Microsoft.EntityFrameworkCore;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL.Repositories;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return DbSet
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetByIdWithSignedDocumentsAsync(long id, CancellationToken cancellationToken = default)
    {
        return DbSet
            .Include(u => u.Documents)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserDocument>> GetUserDocumentsDocumentsAsync(long id,
        CancellationToken cancellationToken = default)
    {
        return (await DbSet
                .Where(u => u.Id == id)
                .Include(u => u.Documents)
                .ThenInclude(ud => ud.Document)
                .SelectMany(u => u.Documents)
                .ToListAsync(cancellationToken)
            ).AsReadOnly();
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(u => u.Id == id, cancellationToken);
    }
}