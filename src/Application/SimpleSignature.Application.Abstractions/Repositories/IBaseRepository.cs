namespace SimpleSignature.Application.Abstractions.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    IUnitOfWork UnitOfWork { get; }
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TEntity>> FilterAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate,
        CancellationToken cancellationToken = default);

    TEntity Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}