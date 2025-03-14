using CloudSales.Domain.Entities;

namespace CloudSales.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task SaveAsync(Subscription subscription, CancellationToken cancellationToken);
    Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken);
    Task<bool> IsUniqueAsync(int accountId, int softwareServiceId, CancellationToken cancellationToken);
    Task<Subscription?> GetAsync(int accountId, int softwareServiceId, CancellationToken cancellationToken);
}
