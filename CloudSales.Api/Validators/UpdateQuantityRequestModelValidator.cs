using CloudSales.Application.Models;
using FluentValidation;

namespace CloudSales.Api.Validators
{
    public class UpdateQuantityRequestModelValidator : AbstractValidator<UpdateQuantityRequestModel>
    {
        public UpdateQuantityRequestModelValidator()
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
        }
    }
}


