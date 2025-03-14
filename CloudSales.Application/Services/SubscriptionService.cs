﻿using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Common;
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

        var result = await subscriptionValidator.ValidateAsync(subscription, options => options.IncludeRuleSets(Constants.Validation.RuleSets.Create), cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        await subscriptionRepository.SaveAsync(subscription, cancellationToken);

        var extSubscriptionId = await ccpService.OrderLicenseAsync(new OrderLicenseRequestModel(softwareService.ExternalId, request.AccountId, request.Quantity, request.ValidTo), cancellationToken);

        subscription.Activate(extSubscriptionId);
        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task UpdateQuantityAsync(int customerId, int id, int quantity, CancellationToken cancellationToken)
    {
        var subscription = await GetSubscriptionAndValidateOwnerAsync(customerId, id, cancellationToken);

        await ValidateAsync(subscription, cancellationToken, Constants.Validation.RuleSets.Create);

        subscription.UpdateQuantity(quantity);

        await ccpService.UpdateLicenseAsync(new UpdateLicenseRequestModel(subscription.ExternalId!.Value, quantity, subscription.ValidTo), cancellationToken);

        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task UpdateExpirationAsync(int customerId, int id, DateTime validTo, CancellationToken cancellationToken)
    {
        var subscription = await GetSubscriptionAndValidateOwnerAsync(customerId, id, cancellationToken);

        await ValidateAsync(subscription, cancellationToken, Constants.Validation.RuleSets.Update);

        subscription.UpdateExpiration(validTo);

        await ccpService.UpdateLicenseAsync(new UpdateLicenseRequestModel(subscription.ExternalId!.Value, subscription.Quantity, validTo), cancellationToken);

        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    public async Task CancelSubscription(int customerId, int id, CancellationToken cancellationToken)
    {
        var subscription = await GetSubscriptionAndValidateOwnerAsync(customerId, id, cancellationToken);
        await ValidateAsync(subscription, cancellationToken, Constants.Validation.RuleSets.Update);

        subscription.Cancel();

        await ccpService.CancelLicenseAsync(new CancelLicenseRequestModel(subscription.ExternalId!.Value), cancellationToken);

        await subscriptionRepository.UpdateAsync(subscription, cancellationToken);
    }

    private async Task ValidateAsync(Subscription subscription, CancellationToken cancellationToken, params string[] rulesets)
    {
        var result = await subscriptionValidator.ValidateAsync(subscription, options => options.IncludeRuleSets(rulesets), cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }

    private async Task<Subscription> GetSubscriptionAndValidateOwnerAsync(int customerId, int id, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException(id, nameof(Subscription));

        ValidateAccountOwner(customerId, subscription.Account);
        return subscription;
    }

    private static void ValidateAccountOwner(int customerId, Account account)
    {
        if (account.CustomerId != customerId)
            throw new UnauthorizedException();
    }
}
