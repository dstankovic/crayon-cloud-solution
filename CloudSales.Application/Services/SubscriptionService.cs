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

        if (account.CustomerId != customerId)
            throw new UnauthorizedException();

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
}
