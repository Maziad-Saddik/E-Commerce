using E_Commerce.Applications.Queries.Contracts;
using E_Commerce.Domain.Queries.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Infrastructure.Queries.Persistence.Repositories
{
    public class Repository<TEntity, TId>(AppDbContext dbContext) : IRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    {
        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            dbContext.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            await Task.CompletedTask;
        }

        public virtual Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
             => dbContext.Set<TEntity>().FindAsync(keyValues: [id], cancellationToken: cancellationToken).AsTask();

        public virtual Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
            => dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();

        public virtual Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken)
            => dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await dbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        public virtual async Task<IEnumerable<TResult>> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> target,
            CancellationToken cancellationToken
        ) => await dbContext.Set<TEntity>()
                .AsNoTracking()
                .Select(target)
                .ToListAsync(cancellationToken);
    }
}
