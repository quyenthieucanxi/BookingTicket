using System.Linq.Expressions;
using Domain.Shared;

namespace Domain.Abstractions;

public interface IRepository<TEntity> where TEntity: class
{
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    Task<TEntity?> GetByIdAsync(object id,string[]? includes = null);

    Task<IEnumerable<TEntity>> GetAllAsync(string[]? includes = null);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria, string[]? includes = null);

    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[]? includes = null);
        
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int take, int skip);

    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int? take, int? skip,
        Expression<Func<TEntity, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);

    void Update(TEntity entity);

    void Update(IEnumerable<TEntity> entities);

    void Remove(Expression<Func<TEntity, bool>> criteria);

    Task Remove(Guid id);

    void Remove(TEntity entity);
    
    void Remove(IEnumerable<TEntity> entities);

   
    
}