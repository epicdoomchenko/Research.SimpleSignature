using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithSignedDocumentsAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserDocument>> GetUserDocumentsDocumentsAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetActivatedUsersAsync(CancellationToken cancellationToken = default);
}