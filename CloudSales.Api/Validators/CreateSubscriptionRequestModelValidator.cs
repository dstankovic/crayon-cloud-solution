using CloudSales.Application.Models;
using FluentValidation;

namespace CloudSales.Api.Validators;

public class CreateSubscriptionRequestModelValidator : AbstractValidator<CreateSubscriptionRequestModel>
{
    public CreateSubscriptionRequestModelValidator()
    {
        RuleFor(x => x.AccountId)
            .GreaterThan(0)
            .WithMessage("Account Id must be greater than 0");

        RuleFor(x => x.SoftwareServiceId)
            .GreaterThan(0)
            .WithMessage("SoftwareService Id must be greater than 0");

        RuleFor(x => x.Quantity)
           .GreaterThan(0)
           .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.ValidTo)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Valid to must be in the future.");
    }
}
