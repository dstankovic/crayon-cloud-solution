using CloudSales.Application.Interfaces;
using CloudSales.Domain.Entities;

namespace CloudSales.Infrastructure.Persistence;

internal class SubscriptionRepository(DataContext dataContext) : ISubscriptionRepository
{
    public async Task SaveAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscription);

        dataContext.Subscriptions.Add(subscription);

        await dataContext.SaveChangesAsync(cancellationToken);
    }
}
