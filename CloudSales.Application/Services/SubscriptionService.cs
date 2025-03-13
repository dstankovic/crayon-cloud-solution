using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;
using CloudSales.Domain.Exceptions;

namespace CloudSales.Application.Services;

public class SubscriptionService(IAccountRepository accountRepository,
                                 IServiceRepository serviceRepository,
                                 ISubscriptionRepository subscriptionRepository,
                                 ICCPApiService ccpService) : ISubscriptionService
{
    public async Task OrderSoftwareLicenseAsync(int accountId, int softwareServiceId, int quantity, DateTime validTo, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAccountAsync(accountId, cancellationToken)
           ?? throw new EntityNotFoundException(accountId, nameof(Account));

        var softwareService = await serviceRepository.GetServiceAsync(softwareServiceId, cancellationToken)
           ?? throw new EntityNotFoundException(softwareServiceId, nameof(SoftwareService));

        var subscription = new Subscription(
            softwareService.Name,
            quantity,
            SubscriptionState.Pending,
            validTo,
            account,
            softwareService
        );

        await subscriptionRepository.SaveAsync(subscription, cancellationToken);

        //Replace with real ext id
        await ccpService.OrderLicenseAsync(new OrderLicenseRequestModel(Guid.NewGuid(), accountId, quantity, validTo), cancellationToken);

        subscription.UpdateState(SubscriptionState.Active);
        await subscriptionRepository.SaveAsync(subscription, cancellationToken);
    }
}
