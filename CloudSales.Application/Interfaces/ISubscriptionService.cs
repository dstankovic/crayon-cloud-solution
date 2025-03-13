namespace CloudSales.Application.Interfaces;

public interface ISubscriptionService
{
    Task OrderSoftwareLicenseAsync(int accountId, int softwareServiceId, int quantity, DateTime validTo, CancellationToken cancellationToken);
}
