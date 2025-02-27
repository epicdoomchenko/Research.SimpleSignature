using Microsoft.EntityFrameworkCore;
using SimpleSignature.Application.Abstractions;
using SimpleSignature.Application.Abstractions.Repositories;

namespace SimpleSignature.Infrastructure.DAL.Repositories;

internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public IUnitOfWork UnitOfWork => DbContext;

    protected BaseRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return (await DbSet.AsNoTracking().ToListAsync(cancellationToken)).AsReadOnly();
    }

    public async Task<IReadOnlyCollection<TEntity>> FilterAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        query = predicate(query);

        return (await query.ToListAsync(cancellationToken)).AsReadOnly();
    }

    public TEntity Add(TEntity entity)
    {
        return DbSet.Add(entity).Entity;
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }
}