using CloudSales.Domain.Entities;

namespace CloudSales.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task SaveAsync(Subscription subscription, CancellationToken cancellationToken);
}
