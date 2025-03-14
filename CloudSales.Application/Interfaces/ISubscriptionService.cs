using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces;

public interface ISubscriptionService
{
    Task CreateSubscriptionAsync(int customerId, CreateSubscriptionRequestModel request, CancellationToken cancellationToken);
    Task UpdateQuantityAsync(int customerId, int accountId, int softwareServiceId, int quantity, CancellationToken cancellationToken);
    Task CancelSubscription(int customerId, int accountId, int softwareServiceId, CancellationToken cancellationToken);
    Task UpdateExpirationAsync(int customerId, int accountId, int softwareServiceId, DateTime validTo, CancellationToken cancellationToken);
}