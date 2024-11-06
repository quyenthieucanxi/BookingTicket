using System.Linq.Expressions;
using Domain.Shared;

namespace Domain.Abstractions;

public interface IRepository<TEntity> where TEntity: class
{
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    Task<TEntity?> GetByIdAsync(object id,CancellationToken  cancellationToken,string[]? includes = null);

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken  cancellationToken,string[]? includes = null);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria,CancellationToken  cancellationToken, string[]? includes = null);

    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria,CancellationToken  cancellationToken, string[]? includes = null);
        
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int take, int skip,CancellationToken  cancellationToken,string[]? includes = null);

    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? criteria,
        CancellationToken  cancellationToken,
        int? take, int? skip,
        Expression<Func<TEntity, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending,
        string[]? includes = null);

    void Update(TEntity entity);

    void Update(IEnumerable<TEntity> entities);

    Task Remove(Expression<Func<TEntity, bool>> criteria,CancellationToken cancellationToken);

    Task Remove(Guid id,CancellationToken cancellationToken);

    void Remove(TEntity entity);
    
    void Remove(IEnumerable<TEntity> entities);

   
    
}