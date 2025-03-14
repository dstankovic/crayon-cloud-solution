using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces;

public interface ISubscriptionService
{
    Task CreateSubscriptionAsync(int customerId, CreateSubscriptionRequestModel request, CancellationToken cancellationToken);
    Task UpdateQuantityAsync(int customerId, int id, int quantity, CancellationToken cancellationToken);
    Task CancelSubscription(int customerId, int id, CancellationToken cancellationToken);
    Task UpdateExpirationAsync(int customerId, int id, DateTime validTo, CancellationToken cancellationToken);
}