using CloudSales.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CloudSales.Infrastructure.Persistence.Interceptors;

public class UpdateTimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateUpdatedOnProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateUpdatedOnProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateUpdatedOnProperties(DbContext? context)
    {
        if (context is null)
            return;

        // Get all entries of type BaseEntity with Modified state
        var baseEntityEntries = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in baseEntityEntries)
            entry.Entity.UpdatedAt = DateTime.UtcNow;

        var baseAggregatedEntityEntries = context.ChangeTracker.Entries<BaseAggregatedEntity>()
           .Where(e => e.State == EntityState.Modified);

        foreach (var entry in baseAggregatedEntityEntries)
            entry.Entity.UpdatedAt = DateTime.UtcNow;
    }
}
