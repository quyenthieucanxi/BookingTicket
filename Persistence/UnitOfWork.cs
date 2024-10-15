using System.Data;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Data;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    
    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public  Task<int> SaveChanges(CancellationToken cancellation = default)
    {
        return  _applicationDbContext.SaveChangesAsync(cancellation);
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _applicationDbContext.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }
}