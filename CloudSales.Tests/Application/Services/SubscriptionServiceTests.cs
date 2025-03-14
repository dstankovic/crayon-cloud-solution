using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Application.Services;
using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;
using CloudSales.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace CloudSales.Tests.Application.Services;

public class SubscriptionServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
    private readonly Mock<IServiceRepository> _serviceRepositoryMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    private readonly Mock<ICCPApiService> _ccpServiceMock = new();
    private readonly Mock<IValidator<Subscription>> _subscriptionValidatorMock = new();

    private readonly SubscriptionService _unitUnderTest;

    public SubscriptionServiceTests()
    {
        _unitUnderTest = new SubscriptionService(
            _accountRepositoryMock.Object,
            _serviceRepositoryMock.Object,
            _subscriptionRepositoryMock.Object,
            _ccpServiceMock.Object,
            _subscriptionValidatorMock.Object
        );
    }

    [Fact]
    public async Task CreateSubscriptionAsync_AccountNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var customerId = 1;
        var request = new CreateSubscriptionRequestModel(1, 1, 1, DateTime.UtcNow.AddMonths(1));

        _accountRepositoryMock.Setup(x => x.GetAccountAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Account?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _unitUnderTest.CreateSubscriptionAsync(customerId, request, CancellationToken.None));

        Assert.Equal("Account", exception.EntityName);
        Assert.Equal(request.AccountId, exception.EntityId);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ServiceNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var customerId = default(int);
        var request = new CreateSubscriptionRequestModel(1, 1, 1, DateTime.UtcNow.AddMonths(1));

        var account = new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone"));
        _accountRepositoryMock.Setup(x => x.GetAccountAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(account);

        _serviceRepositoryMock.Setup(x => x.GetServiceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((SoftwareService?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _unitUnderTest.CreateSubscriptionAsync(customerId, request, CancellationToken.None));

        Assert.Equal("SoftwareService", exception.EntityName);
        Assert.Equal(request.SoftwareServiceId, exception.EntityId);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_CustomerThatIsNotOwnerCreates_UnauthorizedException()
    {
        // Arrange
        var customerId = int.MaxValue;
        var request = new CreateSubscriptionRequestModel(1, 1, 10, DateTime.UtcNow.AddMonths(1));

        var account = new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone"));
        _accountRepositoryMock.Setup(x => x.GetAccountAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(account);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _unitUnderTest.CreateSubscriptionAsync(customerId, request, CancellationToken.None));
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ValidationFails_ThrowsValidationException()
    {
        // Arrange
        var customerId = default(int);
        var request = new CreateSubscriptionRequestModel(1, 1, 0, DateTime.UtcNow.AddMonths(1));

        var account = new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone"));
        var softwareService = new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100);

        _accountRepositoryMock.Setup(x => x.GetAccountAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(account);
        _serviceRepositoryMock.Setup(x => x.GetServiceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(softwareService);

        _subscriptionValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                                  {
                                        new("Quantity", "Must be greater than 0")
                                  }));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _unitUnderTest.CreateSubscriptionAsync(customerId, request, CancellationToken.None));

        Assert.Contains("Quantity", exception.Errors.Select(e => e.PropertyName));
    }

    [Fact]
    public async Task CreateSubscriptionAsync_Success_CreatesSubscription()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var customerId = default(int);
        var request = new CreateSubscriptionRequestModel(1, 1, 10, DateTime.UtcNow.AddMonths(1));

        var account = new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone"));
        var softwareService = new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100);

        _accountRepositoryMock.Setup(x => x.GetAccountAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(account);
        _serviceRepositoryMock.Setup(x => x.GetServiceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(softwareService);

        _subscriptionValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new ValidationResult());

        _ccpServiceMock.Setup(x => x.OrderLicenseAsync(It.IsAny<OrderLicenseRequestModel>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(externalId);

        _subscriptionRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
                                   .Returns(Task.CompletedTask);

        _subscriptionRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

        // Act
        await _unitUnderTest.CreateSubscriptionAsync(customerId, request, CancellationToken.None);

        // Assert
        _subscriptionRepositoryMock.Verify(r => r.SaveAsync(It.Is<Subscription>(sub => sub.AccountId == account.Id && sub.SoftwareServiceId == softwareService.Id), It.IsAny<CancellationToken>()), Times.Once);
        _subscriptionRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Subscription>(sub => sub.AccountId == account.Id && sub.SoftwareServiceId == softwareService.Id && sub.State == SubscriptionState.Active), It.IsAny<CancellationToken>()), Times.Once);
        _ccpServiceMock.Verify(c => c.OrderLicenseAsync(It.IsAny<OrderLicenseRequestModel>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_InvalidSubscription_ThrowsEntityNotFoundException()
    {
        // Arrange
        var customerId = 1;
        var subscriptionId = 999;
        var newQuantity = 10;
        var cancellationToken = new CancellationToken();

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync((Subscription?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _unitUnderTest.UpdateQuantityAsync(customerId, subscriptionId, newQuantity, cancellationToken));

        Assert.Equal("Subscription", exception.EntityName);
        Assert.Equal(subscriptionId, exception.EntityId);
    }

    [Fact]
    public async Task UpdateQuantityAsync_UnauthorizedAccess_ThrowsUnauthorizedException()
    {
        // Arrange
        var customerId = 75;
        var subscriptionId = 123;
        var newQuantity = 10;
        var cancellationToken = new CancellationToken();
        var subscription = GetSubscriptionFake();

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync(subscription);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _unitUnderTest.UpdateQuantityAsync(customerId, subscriptionId, newQuantity, cancellationToken));
    }

    [Fact]
    public async Task UpdateQuantityAsync_ValidSubscription_UpdatesSuccessfully()
    {
        // Arrange
        var customerId = default(int);
        var subscriptionId = 123;
        var newQuantity = 10;
        var cancellationToken = new CancellationToken();
        var externalSubscriptionId = Guid.NewGuid();

        var subscription = GetSubscriptionFake();
        subscription.Activate(externalSubscriptionId);

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync(subscription);

        _subscriptionValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        await _unitUnderTest.UpdateQuantityAsync(customerId, subscriptionId, newQuantity, cancellationToken);

        // Assert
        _ccpServiceMock.Verify(c => c.UpdateLicenseAsync(new UpdateLicenseRequestModel(externalSubscriptionId, newQuantity, subscription.ValidTo), cancellationToken), Times.Once);
        _subscriptionRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Subscription>(sub => sub.Quantity == newQuantity), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateExpirationAsync_InvalidSubscription_ThrowsEntityNotFoundException()
    {
        // Arrange
        var customerId = 1;
        var subscriptionId = 999;
        var newValidTo = DateTime.UtcNow.AddDays(7);
        var cancellationToken = new CancellationToken();

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync((Subscription?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _unitUnderTest.UpdateExpirationAsync(customerId, subscriptionId, newValidTo, cancellationToken));

        Assert.Equal("Subscription", exception.EntityName);
        Assert.Equal(subscriptionId, exception.EntityId);
    }

    [Fact]
    public async Task UpdateExpirationAsync_UnauthorizedAccess_ThrowsUnauthorizedException()
    {
        // Arrange
        var customerId = 75;
        var subscriptionId = 123;
        var newValidTo = DateTime.UtcNow.AddDays(7);
        var cancellationToken = new CancellationToken();

        var subscription = GetSubscriptionFake();

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync(subscription);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _unitUnderTest.UpdateExpirationAsync(customerId, subscriptionId, newValidTo, cancellationToken));
    }

    [Fact]
    public async Task UpdateExpirationAsync_ValidSubscription_UpdatesSuccessfully()
    {
        // Arrange
        var customerId = default(int);
        var subscriptionId = 123;
        var newValidTo = DateTime.UtcNow.AddDays(7);
        var cancellationToken = new CancellationToken();
        var externalSubscriptionId = Guid.NewGuid();

        var subscription = GetSubscriptionFake();
        subscription.Activate(externalSubscriptionId);

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync(subscription);

        _subscriptionValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        await _unitUnderTest.UpdateExpirationAsync(customerId, subscriptionId, newValidTo, cancellationToken);

        // Assert
        _ccpServiceMock.Verify(c => c.UpdateLicenseAsync(new UpdateLicenseRequestModel(externalSubscriptionId, subscription.Quantity, newValidTo), cancellationToken), Times.Once);
        _subscriptionRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Subscription>(sub => sub.ValidTo == newValidTo), cancellationToken), Times.Once);
    }


    [Fact]
    public async Task CancelSubscriptionAsync_InvalidSubscription_ThrowsEntityNotFoundException()
    {
        // Arrange
        var customerId = 1;
        var subscriptionId = 999;
        var cancellationToken = new CancellationToken();

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync((Subscription?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _unitUnderTest.CancelSubscription(customerId, subscriptionId, cancellationToken));

        Assert.Equal("Subscription", exception.EntityName);
        Assert.Equal(subscriptionId, exception.EntityId);
    }

    [Fact]
    public async Task CancelSubscriptionAsync_UnauthorizedAccess_ThrowsUnauthorizedException()
    {
        // Arrange
        var customerId = 75;
        var subscriptionId = 123;
        var cancellationToken = new CancellationToken();

        var subscription = GetSubscriptionFake();

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync(subscription);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _unitUnderTest.CancelSubscription(customerId, subscriptionId, cancellationToken));
    }


    [Fact]
    public async Task CancelSubscriptionAsync_ValidSubscription_CancelsSuccessfully()
    {
        // Arrange
        var customerId = default(int);
        var subscriptionId = 123;
        var cancellationToken = new CancellationToken();
        var extSubId = Guid.NewGuid();

        var subscription = GetSubscriptionFake();
        subscription.Activate(extSubId);

        _subscriptionRepositoryMock.Setup(r => r.GetAsync(subscriptionId, cancellationToken))
            .ReturnsAsync(subscription);

        _subscriptionValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        await _unitUnderTest.CancelSubscription(customerId, subscriptionId, cancellationToken);

        // Assert
        _ccpServiceMock.Verify(c => c.CancelLicenseAsync(new CancelLicenseRequestModel(extSubId), cancellationToken), Times.Once);
        _subscriptionRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Subscription>(sub => sub.State == SubscriptionState.Cancelled), cancellationToken), Times.Once);
    }

    private static Subscription GetSubscriptionFake()
    {
        return new Subscription(
                    "SoftwareService",
                    5,
                    SubscriptionState.Active,
                    DateTime.UtcNow,
                    new Account("Test Account", "Test Desc", new Customer("CustName", "email", "phone")),
                    new SoftwareService(Guid.NewGuid(), "Test Service", "Description", 100));
    }
}
