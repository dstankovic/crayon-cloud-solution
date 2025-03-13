using CloudSales.Application.Interfaces;
using CloudSales.Domain.Entities;
using FluentValidation;

namespace CloudSales.Application.Validators;

public class SubscriptionValidator : AbstractValidator<Subscription>
{
    public SubscriptionValidator(ISubscriptionRepository subscriptionRepository)
    {
        RuleFor(x => new { x.SoftwareServiceId, x.AccountId })
           .MustAsync(async (ids, cancellationToken) => await subscriptionRepository.IsUniqueAsync(ids.AccountId, ids.SoftwareServiceId, cancellationToken))
           .WithMessage("Subscription must be unique. Try to adjust quantity or expiration date.");
    }
}
