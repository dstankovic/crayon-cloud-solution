using CloudSales.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using CloudSales.Application.Services;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;


namespace CloudSales.Tests.Application.Services;

public class ServiceSyncServiceTests
{
    private readonly Mock<ICCPApiService> _ccpApiServiceMock = new();
    private readonly Mock<IServiceRepository> _serviceRepositoryMock = new();
    private readonly Mock<ILogger<ServiceSyncService>> _loggerMock = new();

    private readonly ServiceSyncService _unitUnderTest;

    public ServiceSyncServiceTests()
    {

        _unitUnderTest = new ServiceSyncService(_ccpApiServiceMock.Object, _loggerMock.Object, _serviceRepositoryMock.Object);
    }

    [Fact]
    public async Task ProcessSoftwareServicesBatchAsync_Should_Add_New_Services_If_They_Do_Not_Exist()
    {
        // Arrange
        var softwareServices = new List<SoftwareServiceResponseModel>
        {
            new ( Guid.NewGuid(), "Service 1", "Desc 1", 10 ),
        };

        _ccpApiServiceMock.Setup(x => x.GetAvailableSoftwareServicesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(softwareServices);

        var existingServices = new List<SoftwareService>();
        _serviceRepositoryMock.Setup(x => x.GetAllByExternalIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(existingServices);

        // Act
        await _unitUnderTest.SyncSoftwareServicesAsync(CancellationToken.None);

        // Assert
        _serviceRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<List<SoftwareService>>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceRepositoryMock.Verify(x => x.UpdateRangeAsync(It.IsAny<IEnumerable<SoftwareService>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessSoftwareServicesBatchAsync_Should_Update_Existing_Services_If_They_Exist()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var softwareServices = new List<SoftwareServiceResponseModel>
        {
            new ( existingId, "Service 1", "Desc 1", 10 ),
        };

        var existingServices = new List<SoftwareService>
        {
            new (existingId, "Old Service", "Old Desc", 10)
        };

        _ccpApiServiceMock.Setup(x => x.GetAvailableSoftwareServicesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(softwareServices);

        _serviceRepositoryMock.Setup(x => x.GetAllByExternalIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingServices);

        // Act
        await _unitUnderTest.SyncSoftwareServicesAsync(CancellationToken.None);

        // Assert
        _serviceRepositoryMock.Verify(x => x.UpdateRangeAsync(It.IsAny<IEnumerable<SoftwareService>>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<List<SoftwareService>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessSoftwareServicesBatchAsync_Should_Mark_Services_As_Inactive_If_They_Exist_In_DB_But_Not_Received_From_CCP()
    {
        // Arrange
        var service1IdMock = Guid.NewGuid();
        var existingServices = new List<SoftwareService>
        {
            new (service1IdMock, "Old Service 1", "Old Desc 1", 10),
            new (Guid.NewGuid(), "Old Service", "Old Desc 2", 20)
        };

        _ccpApiServiceMock.Setup(x => x.GetAvailableSoftwareServicesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SoftwareServiceResponseModel> { new(service1IdMock, "Test", "test", 5) });

        _serviceRepositoryMock.Setup(x => x.GetAllByExternalIdsAsync(It.IsAny<HashSet<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingServices);

        // Act
        await _unitUnderTest.SyncSoftwareServicesAsync(CancellationToken.None);

        // Assert
        _serviceRepositoryMock.Verify(x => x.UpdateRangeAsync(It.Is<IEnumerable<SoftwareService>>(x => x.Any(software => software.State == CloudSales.Domain.Enums.SoftwareServiceState.Inactive)), It.IsAny<CancellationToken>()), Times.Once);
        _serviceRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<List<SoftwareService>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
