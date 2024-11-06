using System.Linq.Expressions;
using Domain.Abstractions;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _applicationDbContext;
    public DbSet<TEntity> Entities { get; set; }

    public Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        Entities = _applicationDbContext.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        _applicationDbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        _applicationDbContext.Set<TEntity>().AddRange(entities);
    }
    
    public async Task<TEntity?> GetByIdAsync(object id,CancellationToken  cancellationToken,string[]? includes = null)
    {
        return await _applicationDbContext.Set<TEntity>().FindAsync(id,cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken  cancellationToken,string[]? includes = null)
    {
        IQueryable<TEntity> query = _applicationDbContext.Set<TEntity>();
        if (includes != null)
            foreach (var incluse in includes)
                query = query.Include(incluse);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria,CancellationToken  cancellationToken, string[]? includes = null)
    {
        IQueryable<TEntity> query = _applicationDbContext.Set<TEntity>();

        if (includes != null)
            foreach (var incluse in includes)
                query = query.Include(incluse);
            
        return await query.SingleOrDefaultAsync(criteria,cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria,CancellationToken  cancellationToken, string[]? includes = null)
    {
        IQueryable<TEntity> query = _applicationDbContext.Set<TEntity>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.Where(criteria).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int take, int skip,CancellationToken  cancellationToken,string[]? includes = null)
    {
        IQueryable<TEntity> query = _applicationDbContext.Set<TEntity>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        return await query.Where(criteria).Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? criteria,CancellationToken  cancellationToken,
        int? take, int? skip,
        Expression<Func<TEntity, object>>? orderBy = null,
        string? orderByDirection = OrderBy.Ascending,
        string[]? includes = null
        )
    {
        IQueryable<TEntity> query = _applicationDbContext.Set<TEntity>();
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        if (criteria is not null)
        {
            query = query.Where(criteria);
        }
        if (orderBy != null)
        {
            if (orderByDirection == OrderBy.Ascending)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderByDescending(orderBy);
        }
        if (take.HasValue)
            query = query.Take(take.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);
        
        return await query.ToListAsync(cancellationToken);
    }


    public void Update(TEntity entity)
    {
        _applicationDbContext.Set<TEntity>().Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        _applicationDbContext.Set<TEntity>().UpdateRange(entities);
    }

    public async Task Remove(Expression<Func<TEntity, bool>> criteria,CancellationToken cancellationToken)
    {
        var entity = await FindAsync(criteria,cancellationToken);
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        Remove(entity);
    }

    public async Task Remove(Guid id,CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id,cancellationToken);
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        Remove(entity);
    }

    public void Remove(TEntity entity)
    {
        _applicationDbContext.Set<TEntity>().Remove(entity);
    }
    

    public void Remove(IEnumerable<TEntity> entities)
    {
        _applicationDbContext.Set<TEntity>().RemoveRange(entities);
    }
}