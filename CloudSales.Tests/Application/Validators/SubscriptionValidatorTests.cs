using CloudSales.Application.Interfaces;
using CloudSales.Application.Validators;
using CloudSales.Domain.Common;
using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;
using FluentValidation;
using Moq;


namespace CloudSales.Tests.Application.Validators;

public class SubscriptionValidatorTests
{
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    private readonly SubscriptionValidator _validator;

    public SubscriptionValidatorTests()
    {
        _validator = new SubscriptionValidator(_subscriptionRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_CreateSubscription_UniqueSubscription_ReturnsNoValidationErrors()
    {
        // Arrange
        var model = new Subscription(
                    "SoftwareService",
                    5,
                    SubscriptionState.Active,
                    DateTime.UtcNow,
                    new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone")),
                    new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100));

        _subscriptionRepositoryMock.Setup(repo => repo.IsUniqueAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.ValidateAsync(model, options => options.IncludeRuleSets(Constants.Validation.RuleSets.Create), CancellationToken.None);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Validate_CreateSubscription_DuplicateSubscription_ReturnsValidationError()
    {
        // Arrange
        var model = new Subscription(
                     "SoftwareService",
                     5,
                     SubscriptionState.Active,
                     DateTime.UtcNow,
                     new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone")),
                     new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100));

        _subscriptionRepositoryMock.Setup(repo => repo.IsUniqueAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.ValidateAsync(model, options => options.IncludeRuleSets(Constants.Validation.RuleSets.Create));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Validate_UpdateSubscription_WithCancelledState_ReturnsValidationError()
    {
        // Arrange
        var model = new Subscription(
                    "SoftwareService",
                    5,
                    SubscriptionState.Cancelled,
                    DateTime.UtcNow,
                    new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone")),
                    new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100),
                    Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(model, options => options.IncludeRuleSets(Constants.Validation.RuleSets.Update));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Validate_UpdateSubscription_WithoutExternalId_ReturnsValidationError()
    {
        // Arrange
        var model = new Subscription(
                    "SoftwareService",
                    5,
                    SubscriptionState.Active,
                    DateTime.UtcNow,
                    new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone")),
                    new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100));

        // Act
        var result = await _validator.ValidateAsync(model, options => options.IncludeRuleSets(Constants.Validation.RuleSets.Update));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Validate_UpdateSubscription_WithValidStateAndExternalId_ReturnsNoValidationErrors()
    {
        // Arrange
        var model = new Subscription(
                    "SoftwareService",
                    5,
                    SubscriptionState.Active,
                    DateTime.UtcNow,
                    new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone")),
                    new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100),
                    Guid.NewGuid());

        // Act
        var result = await _validator.ValidateAsync(model, options => options.IncludeRuleSets(Constants.Validation.RuleSets.Update));

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
