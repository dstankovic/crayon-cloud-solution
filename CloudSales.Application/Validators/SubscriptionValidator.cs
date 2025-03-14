using CloudSales.Application.Interfaces;
using CloudSales.Domain.Common;
using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;
using FluentValidation;

namespace CloudSales.Application.Validators;

public class SubscriptionValidator : AbstractValidator<Subscription>
{
    public SubscriptionValidator(ISubscriptionRepository subscriptionRepository)
    {
        RuleSet(Constants.Validation.RuleSets.Create, () =>
        {
            RuleFor(x => new { x.SoftwareServiceId, x.AccountId })
               .MustAsync(async (ids, cancellationToken) => await subscriptionRepository.IsUniqueAsync(ids.AccountId, ids.SoftwareServiceId, cancellationToken))
               .WithMessage("Subscription must be unique. Try to adjust quantity or expiration date.");
        });

        RuleSet(Constants.Validation.RuleSets.Update, () =>
        {
            RuleFor(x => x.State)
               .NotEqual(SubscriptionState.Cancelled)
               .WithMessage("Subscription is cancelled.");

            RuleFor(x => x.ExternalId)
              .NotEmpty()
              .WithMessage("Subscription can't be updated without external id.");
        });
    }
}
