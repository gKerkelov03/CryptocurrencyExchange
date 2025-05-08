using System.Linq.Expressions;
using Application.Abstractions;
using Application.Domain.Base;
using Application.Errors;
using Microsoft.EntityFrameworkCore;
using SmartSalon.Application.ResultObject;

namespace Data;

public class Repository<TEntity>(DatabaseContext _dbContext) : IEfRepository<TEntity>
    where TEntity : class, IBaseEntity
{
    protected readonly DbSet<TEntity> _dbSet = _dbContext.Set<TEntity>();

    public IQueryable<TEntity> All => _dbSet;

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.IncludeAll().FirstOrDefaultAsync(predicate);

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.IncludeAll().FirstOrDefault(predicate);

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.IncludeAll().Where(predicate).ToListAsync();

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

    public async Task<Result> RemoveByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
        {
            return Error.NotFound;
        }

        _dbSet.Remove(entity);

        return Result.Success();
    }

    public async Task<Result> UpdateByIdAsync(Guid id, TEntity newEntity)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
        {
            return Error.NotFound;
        }

        _dbContext.Entry(entity).CurrentValues.SetValues(newEntity);
        return Result.Success();
    }

    public void Update(TEntity newEntity) => _dbSet.Update(newEntity);
}

public static class DbSetExtensions
{
    public static IQueryable<TEntity> IncludeAll<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
    {
        var query = dbSet.AsQueryable();
        var entityType = typeof(TEntity);
        var properties = entityType.GetProperties()
            .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string));

        foreach (var property in properties)
        {
            query = query.Include(property.Name);
        }

        return query;
    }
}