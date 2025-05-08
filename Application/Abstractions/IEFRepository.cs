using System.Linq.Expressions;
using Application.Domain.Base;
using SmartSalon.Application.ResultObject;

namespace Application.Abstractions;

public interface IEfRepository<TEntity> : ITransientLifetime where TEntity : IBaseEntity
{
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    IQueryable<TEntity> All { get; }
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

    void Update(TEntity newEntity);
    Task<Result> UpdateByIdAsync(Guid id, TEntity newEntity);

    Task<Result> RemoveByIdAsync(Guid id);

    Task SaveChangesAsync();
}