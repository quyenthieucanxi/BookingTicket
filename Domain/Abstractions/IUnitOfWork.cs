using System.Data;

namespace Domain.Abstractions;

public interface IUnitOfWork 
{
    Task<int> SaveChanges(CancellationToken cancellation = default);
    IDbTransaction BeginTransaction();
}