using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;

namespace CloudSales.Infrastructure.Services
{
    internal class CCPApiService : ICCPApiService
    {
        public async Task<IEnumerable<SoftwareServiceResponseModel>> GetAvailableSoftwareServicesAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(300, cancellationToken);

            return Enumerable.Empty<SoftwareServiceResponseModel>();
        }

        public async Task OrderLicenseAsync(OrderLicenseRequestModel request, CancellationToken cancellationToken)
        {
            await Task.Delay(300, cancellationToken);
        }
    }
}
