using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interceptors;

public sealed  class AddOrUpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        IEnumerable<EntityEntry<AuditableEntity>> entries = dbContext.ChangeTracker.Entries<AuditableEntity>();

        foreach (EntityEntry<AuditableEntity> entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.ModifiedOn).CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedOn).CurrentValue = DateTime.UtcNow;
            }
            
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}