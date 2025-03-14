using CloudSales.Api;
using CloudSales.Domain.Exceptions;
using System.Net;

namespace CloudSales.Tests.Api;

public class ErrorFactoryTests
{
    private readonly ErrorFactory _errorFactory;

    public ErrorFactoryTests()
    {
        _errorFactory = new ErrorFactory();
    }

    [Fact]
    public void GetErrorResponse_UnauthorizedException_ReturnsCorrectErrorResponse()
    {
        // Arrange
        var exception = new UnauthorizedException("Unauthorized action");

        // Act
        var result = _errorFactory.GetErrorResponse(exception);

        // Assert
        Assert.Equal((int)HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Equal("Unauthorized.", result.Message);
    }

    [Fact]
    public void GetErrorResponse_EntityNotFoundException_ReturnsCorrectErrorResponse()
    {
        // Arrange
        var exception = new EntityNotFoundException("Entity not found");

        // Act
        var result = _errorFactory.GetErrorResponse(exception);

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal("Not found.", result.Message);
        Assert.Equal("Entity not found", result.Details);
    }

    [Fact]
    public void GetErrorResponse_OtherException_ReturnsCorrectErrorResponse()
    {
        // Arrange
        var exception = new InvalidOperationException("An unexpected error occurred");

        // Act
        var result = _errorFactory.GetErrorResponse(exception);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal("An unexpected error occurred.", result.Message);
        Assert.Equal("An unexpected error occurred", result.Details);
    }
}
