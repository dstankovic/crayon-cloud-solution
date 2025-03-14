using CloudSales.Application.Models;
using FluentValidation;

namespace CloudSales.Api.Validators
{
    public class UpdateExpirationRequestModelValidator : AbstractValidator<UpdateExpirationRequestModel>
    {
        public UpdateExpirationRequestModelValidator()
        {
            RuleFor(x => x.ValidTo)
               .GreaterThan(DateTime.UtcNow)
               .WithMessage("Valid to must be in the future.");
        }
    }
}


