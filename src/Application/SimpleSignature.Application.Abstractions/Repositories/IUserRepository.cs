using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}