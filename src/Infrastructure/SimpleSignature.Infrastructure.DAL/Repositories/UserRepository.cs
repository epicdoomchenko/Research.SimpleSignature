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
        return DbSet.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(u => u.Id == id, cancellationToken);
    }
}