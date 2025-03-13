using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces;

public interface ICCPApiService
{
    Task<IEnumerable<SoftwareServiceResponseModel>> GetAvailableSoftwareServicesAsync(CancellationToken cancellationToken);
    Task OrderLicenseAsync(OrderLicenseRequestModel request, CancellationToken cancellationToken);
}
