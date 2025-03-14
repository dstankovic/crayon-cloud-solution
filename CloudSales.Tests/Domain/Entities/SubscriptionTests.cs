using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;

namespace CloudSales.Tests.Domain.Entities;

public class SubscriptionTests
{
    [Fact]
    public void Activate_ValidExternalId_ShouldActivateSubscription()
    {
        // Arrange
        var subscription = GetSubscription();
        var externalId = Guid.NewGuid();

        // Act
        subscription.Activate(externalId);

        // Assert
        Assert.Equal(SubscriptionState.Active, subscription.State);
        Assert.Equal(externalId, subscription.ExternalId);
    }

    [Fact]
    public void Cancel_ShouldSetStateToCancelled()
    {
        // Arrange
        var subscription = GetSubscription();

        // Act
        subscription.Cancel();

        // Assert
        Assert.Equal(SubscriptionState.Cancelled, subscription.State);
    }

    [Fact]
    public void UpdateQuantity_ValidQuantity_ShouldUpdateQuantity()
    {
        // Arrange
        var subscription = GetSubscription();

        // Act
        subscription.UpdateQuantity(20);

        // Assert
        Assert.Equal(20, subscription.Quantity);
    }

    [Fact]
    public void UpdateQuantity_InvalidQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var subscription = GetSubscription();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => subscription.UpdateQuantity(-5));
    }

    [Fact]
    public void UpdateExpiration_ValidExpiration_ShouldUpdateValidTo()
    {
        // Arrange
        var subscription = GetSubscription();

        var newExpiration = DateTime.UtcNow.AddMonths(2);

        // Act
        subscription.UpdateExpiration(newExpiration);

        // Assert
        Assert.Equal(newExpiration, subscription.ValidTo);
    }

    [Fact]
    public void UpdateExpiration_InvalidExpiration_ShouldThrowArgumentException()
    {
        // Arrange
        var subscription = GetSubscription();

        var newExpiration = DateTime.UtcNow.AddMonths(1).AddDays(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => subscription.UpdateExpiration(newExpiration));
    }

    [Fact]
    public void UpdateExpiration_WithoutUTCDateTime_ShouldConvertToUTC()
    {
        // Arrange
        var localDateTime = DateTime.SpecifyKind(DateTime.Now.AddYears(1), DateTimeKind.Local); // Local time
        var unspecifiedDateTime = DateTime.SpecifyKind(DateTime.Now.AddYears(1), DateTimeKind.Unspecified); // Unspecified time

        var subscription = GetSubscription();

        // Act
        subscription.UpdateExpiration(localDateTime);
        var subscriptionWithLocalTimeUpdated = subscription.ValidTo;

        subscription.UpdateExpiration(unspecifiedDateTime);
        var subscriptionWithUnspecifiedTimeUpdated = subscription.ValidTo;

        // Assert
        Assert.Equal(DateTimeKind.Utc, subscriptionWithLocalTimeUpdated.Kind);
        Assert.Equal(DateTimeKind.Utc, subscriptionWithUnspecifiedTimeUpdated.Kind);
    }

    private static Subscription GetSubscription()
    {
        return new Subscription(
                    "Test Subscription",
                    10,
                    SubscriptionState.Pending,
                    DateTime.UtcNow.AddMonths(1),
                    null!,
                    null!
                );
    }
}
