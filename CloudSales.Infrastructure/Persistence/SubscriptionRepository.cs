using CloudSales.Application.Interfaces;
using CloudSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class SubscriptionRepository(DataContext dataContext) : ISubscriptionRepository
{
    public async Task<bool> IsUniqueAsync(int accountId, int softwareServiceId, CancellationToken cancellationToken)
    {
        return !await dataContext.Subscriptions.AnyAsync(sub => sub.SoftwareServiceId == softwareServiceId && sub.AccountId == accountId, cancellationToken);
    }

    public async Task SaveAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscription);

        dataContext.Subscriptions.Add(subscription);

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscription);

        dataContext.Subscriptions.Update(subscription);

        await dataContext.SaveChangesAsync(cancellationToken);
    }
}
