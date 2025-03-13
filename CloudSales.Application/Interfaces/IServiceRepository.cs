using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<ServiceViewModel>> GetServicesModelsAsync();
}
