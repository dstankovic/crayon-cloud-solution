using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class ServiceRepository(DataContext dataContext) : IServiceRepository
{
    public async Task<IEnumerable<ServiceViewModel>> GetServicesModelsAsync()
    {
        return await dataContext.SoftwareServices.Select(service => new ServiceViewModel(service.Name, service.Description, service.PricePerLicense)).ToListAsync();
    }
}
