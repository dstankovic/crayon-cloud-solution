using CloudSales.Application.Models;
using CloudSales.Domain.Entities;

namespace CloudSales.Application.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<ServiceViewModel>> GetServicesModelsAsync(CancellationToken cancellationToken);
    Task<SoftwareService?> GetServiceAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<SoftwareService>> GetAllByExternalIdsAsync(HashSet<Guid> externalIds, CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<SoftwareService> softwareServices, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<SoftwareService> softwareServices, CancellationToken cancellationToken);
}
