using CloudSales.Application.Models;
using CloudSales.Domain.Entities;

namespace CloudSales.Application.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<ServiceViewModel>> GetServicesModelsAsync(CancellationToken cancellationToken);
    Task<SoftwareService?> GetServiceAsync(int id, CancellationToken cancellationToken);
}
