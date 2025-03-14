using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;

namespace CloudSales.Infrastructure.Services
{
    internal class CCPApiService : ICCPApiService
    {
        public async Task CancelLicenseAsync(CancelLicenseRequestModel request, CancellationToken cancellationToken)
        {
            await Task.Delay(300, cancellationToken);
        }

        public async Task<Guid> OrderLicenseAsync(OrderLicenseRequestModel request, CancellationToken cancellationToken)
        {
            await Task.Delay(300, cancellationToken);
            return Guid.NewGuid();
        }

        public async Task UpdateLicenseAsync(UpdateLicenseRequestModel request, CancellationToken cancellationToken)
        {
            await Task.Delay(300, cancellationToken);
        }

        public async Task<IEnumerable<SoftwareServiceResponseModel>> GetAvailableSoftwareServicesAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(300, cancellationToken);

            return Enumerable.Range(0, 50)
                .Select(i => new SoftwareServiceResponseModel(Guid.Parse($"{i.ToString("D2")}c4e47f-dc59-480a-9147-51bd0334b709"), $"Service Mock {i}", $"Service Mock Description {i}", 100m + 2 * i))
                .ToList();

        }
    }
}
