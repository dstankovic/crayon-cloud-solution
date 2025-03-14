using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;
using CloudSales.Domain.Exceptions;
using FluentValidation;

namespace CloudSales.Application.Services;

public class SubscriptionService(IAccountRepository accountRepository,
                                 IServiceRepository serviceRepository,
                                 ISubscriptionRepository subscriptionRepository,
                                 ICCPApiService ccpService,
                                 IValidator<Subscription> subscriptionValidator) : ISubscriptionService
{
    public async Task CreateSubscriptionAsync(int customerId, CreateSubscriptionRequestModel request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAccountAsync(request.AccountId, cancellationToken)
           ?? throw new EntityNotFoundException(request.AccountId, nameof(Account));

        ValidateAccountOwner(customerId, account);

        var softwareService = await serviceRepository.GetServiceAsync(request.SoftwareServiceId, cancellationToken)
           ?? throw new EntityNotFoundException(request.SoftwareServiceId, nameof(SoftwareService));

        var subscription = new Subscription(
            softwareService.Name,
            request.Quantity,
            SubscriptionState.Pending,
            request.ValidTo,
            account,
            softwareService
        );

        await subscriptionValidator.ValidateAndThrowAsync(subscription, cancellationToken);

        await subscriptionRepository.SaveAsync(subscription, cancellationToken);

        //Replace with real ext id
        await ccpService.OrderLicenseAsync(new OrderLicenseRequestModel(Guid.NewGuid(), request.AccountId, request.Quantity, request.ValidTo), cancellationToken);

        subscription.UpdateState(SubscriptionState.Active);
        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task UpdateQuantityAsync(int customerId, int accountId, int softwareServiceId, int quantity, CancellationToken cancellationToken)
    {
        var subscription = await GetSubscriptionAndValidateOwnerAsync(customerId, accountId, softwareServiceId, cancellationToken);

        subscription.UpdateQuantity(quantity);

        await ccpService.UpdateLicenseAsync(new UpdateLicenseRequestModel(Guid.NewGuid(), accountId, quantity, subscription.ValidTo), cancellationToken);

        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task UpdateExpirationAsync(int customerId, int accountId, int softwareServiceId, DateTime validTo, CancellationToken cancellationToken)
    {
        var subscription = await GetSubscriptionAndValidateOwnerAsync(customerId, accountId, softwareServiceId, cancellationToken);

        subscription.UpdateExpiration(validTo);

        await ccpService.UpdateLicenseAsync(new UpdateLicenseRequestModel(Guid.NewGuid(), accountId, subscription.Quantity, validTo), cancellationToken);

        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task CancelSubscription(int customerId, int accountId, int softwareServiceId, CancellationToken cancellationToken)
    {
        var subscription = await GetSubscriptionAndValidateOwnerAsync(customerId, accountId, softwareServiceId, cancellationToken);

        subscription.UpdateState(SubscriptionState.Cancelled);

        await ccpService.CancelLicenseAsync(new CancelLicenseRequestModel(Guid.NewGuid(), accountId), cancellationToken);

        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    private async Task<Subscription> GetSubscriptionAndValidateOwnerAsync(int customerId, int accountId, int softwareServiceId, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetAsync(accountId, softwareServiceId, cancellationToken)
                   ?? throw new EntityNotFoundException("Subscription could not be found.");

        ValidateAccountOwner(customerId, subscription.Account);
        return subscription;
    }

    private static void ValidateAccountOwner(int customerId, Account account)
    {
        if (account.CustomerId != customerId)
            throw new UnauthorizedException();
    }
}
