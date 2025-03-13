using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class ServiceRepository(DataContext dataContext) : IServiceRepository
{
    public async Task<SoftwareService?> GetServiceAsync(int id, CancellationToken cancellationToken)
    {
        return await dataContext.SoftwareServices.FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ServiceViewModel>> GetServicesModelsAsync(CancellationToken cancellationToken)
    {
        return await dataContext.SoftwareServices.Select(service => new ServiceViewModel(service.Name, service.Description, service.PricePerLicense)).ToListAsync(cancellationToken);
    }
}
