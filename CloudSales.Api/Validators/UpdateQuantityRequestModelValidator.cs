using CloudSales.Application.Models;
using FluentValidation;

namespace CloudSales.Api.Validators
{
    public class UpdateQuantityRequestModelValidator : AbstractValidator<UpdateQuantityRequestModel>
    {
        public UpdateQuantityRequestModelValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThan(0)
               .WithMessage("Quantity must be greater than 0");
        }
    }
}


