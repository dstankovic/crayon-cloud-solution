using System.Security.Claims;
using CloudSales.Api.Extensions;

namespace CloudSales.Tests.Api.Extensions;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetIntValueFromClaims_ValidClaim_ReturnsExpectedValue()
    {
        // Arrange
        var claimName = "CustomerId";
        var expectedValue = 123;

        var claims = new List<Claim>
        {
            new(claimName, expectedValue.ToString())
        };

        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = user.GetIntValueFromClaims(claimName);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void GetIntValueFromClaims_InvalidClaim_ReturnsNull()
    {
        // Arrange
        var claimName = "CustomerId";
        var invalidValue = "invalid";  // This should fail to parse as an integer

        var claims = new List<Claim>
            {
                new Claim(claimName, invalidValue)
            };

        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = user.GetIntValueFromClaims(claimName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetIntValueFromClaims_ClaimDoesNotExist_ReturnsNull()
    {
        // Arrange
        var claimName = "NonExistingClaim";

        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = user.GetIntValueFromClaims(claimName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetIntValueFromClaims_ClaimIsEmpty_ReturnsNull()
    {
        // Arrange
        var claimName = "CustomerId";

        var claims = new List<Claim>
            {
                new(claimName, "")
            };

        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = user.GetIntValueFromClaims(claimName);

        // Assert
        Assert.Null(result);
    }
}
