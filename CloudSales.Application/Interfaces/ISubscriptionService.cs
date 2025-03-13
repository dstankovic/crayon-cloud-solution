using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces;

public interface ISubscriptionService
{
    Task CreateSubscriptionAsync(int customerId, CreateSubscriptionRequestModel request, CancellationToken cancellationToken);
}
