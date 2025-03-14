using CloudSales.Api.Validators;
using CloudSales.Application.Models;
using FluentValidation;

namespace CloudSales.Tests.Api.Validators;

public class CreateSubscriptionRequestModelValidatorTests : AbstractValidator<CreateSubscriptionRequestModel>
{
    private readonly CreateSubscriptionRequestModelValidator _validator;

    public CreateSubscriptionRequestModelValidatorTests()
    {
        _validator = new CreateSubscriptionRequestModelValidator();
    }

    [Fact]
    public void Validate_ValidModel_ShouldNotHaveAnyValidationError()
    {
        // Arrange
        var model = new CreateSubscriptionRequestModel(1, 1, 10, DateTime.UtcNow.AddDays(1));

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidAccountId_ShouldHaveValidationError()
    {
        // Arrange
        var model = new CreateSubscriptionRequestModel(0, 1, 10, DateTime.UtcNow.AddDays(1));

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Account Id must be greater than 0", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_InvalidSoftwareServiceId_ShouldHaveValidationError()
    {
        // Arrange
        var model = new CreateSubscriptionRequestModel(1, 0, 10, DateTime.UtcNow.AddDays(1));

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("SoftwareService Id must be greater than 0", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_InvalidQuantity_ShouldHaveValidationError()
    {
        // Arrange
        var model = new CreateSubscriptionRequestModel(1, 1, 0, DateTime.UtcNow.AddDays(1));

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Quantity must be greater than 0", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_InvalidValidTo_ShouldHaveValidationError()
    {
        // Arrange
        var model = new CreateSubscriptionRequestModel(1, 1, 1, DateTime.UtcNow.AddDays(-2));

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Valid to must be in the future.", result.Errors[0].ErrorMessage);
    }
}
