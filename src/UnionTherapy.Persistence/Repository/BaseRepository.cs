using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Common;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    protected readonly BaseDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(BaseDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    protected IQueryable<TEntity> Query() => _dbSet;

    // Gelişmiş GetAsync metodu
    public virtual async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
            
        if (include != null)
            queryable = include(queryable);
            
        if (!withDeleted)
            queryable = queryable.Where(e => !e.IsDeleted);
            
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    // Gelişmiş GetListAsync metodu
    public virtual async Task<IEnumerable<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
            
        if (include != null)
            queryable = include(queryable);
            
        if (!withDeleted)
            queryable = queryable.Where(e => !e.IsDeleted);
            
        if (predicate != null)
            queryable = queryable.Where(predicate);
            
        if (orderBy != null)
            queryable = orderBy(queryable);
            
        return await queryable.ToListAsync(cancellationToken);
    }

    // Gelişmiş sayfalama metodu
    public virtual async Task<IEnumerable<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 20,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
            
        if (include != null)
            queryable = include(queryable);
            
        if (!withDeleted)
            queryable = queryable.Where(e => !e.IsDeleted);
            
        if (predicate != null)
            queryable = queryable.Where(predicate);
            
        if (orderBy != null)
            queryable = orderBy(queryable);
            
        return await queryable.Skip(index * size).Take(size).ToListAsync(cancellationToken);
    }

    // Mevcut basit metodlar (geriye uyumluluk için)
    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await GetAsync(e => e.Id!.Equals(id), cancellationToken: cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await GetAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Query().Where(e => !e.IsDeleted).AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = Query().Where(e => !e.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        
        return await query.CountAsync(cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var entityList = entities.ToList();
        foreach (var entity in entityList)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        
        await _dbSet.AddRangeAsync(entityList, cancellationToken);
        return entityList;
    }

    public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public virtual Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var entityList = entities.ToList();
        foreach (var entity in entityList)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
        
        _dbSet.UpdateRange(entityList);
        return Task.FromResult<IEnumerable<TEntity>>(entityList);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            await DeleteAsync(entity, cancellationToken);
        }
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var entityList = entities.ToList();
        foreach (var entity in entityList)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        
        _dbSet.UpdateRange(entityList);
        await Task.CompletedTask;
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}

// Guid için varsayılan implementasyon
public class BaseRepository<TEntity> : BaseRepository<TEntity, Guid>, IBaseRepository<TEntity> where TEntity : BaseEntity<Guid>
{
    public BaseRepository(BaseDbContext context) : base(context)
    {
    }
} 