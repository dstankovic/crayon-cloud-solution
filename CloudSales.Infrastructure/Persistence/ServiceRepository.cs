using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class ServiceRepository(DataContext dataContext) : IServiceRepository
{
    public async Task AddRangeAsync(IEnumerable<SoftwareService> softwareServices, CancellationToken cancellationToken)
    {
        if (!softwareServices.Any())
            throw new ArgumentException("Empty collection provided", nameof(softwareServices));

        dataContext.SoftwareServices.AddRange(softwareServices);

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<SoftwareService> softwareServices, CancellationToken cancellationToken)
    {
        if (!softwareServices.Any())
            throw new ArgumentException("Empty collection provided", nameof(softwareServices));

        dataContext.SoftwareServices.UpdateRange(softwareServices);

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<SoftwareService>> GetAllByExternalIdsAsync(HashSet<Guid> externalIds, CancellationToken cancellationToken)
    {
        return await dataContext.SoftwareServices.Where(service => externalIds.Contains(service.ExternalId)).ToListAsync(cancellationToken);
    }

    public async Task<SoftwareService?> GetServiceAsync(int id, CancellationToken cancellationToken)
    {
        return await dataContext.SoftwareServices.FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ServiceViewModel>> GetServicesModelsAsync(CancellationToken cancellationToken)
    {
        return await dataContext.SoftwareServices.Select(service => new ServiceViewModel(service.Name, service.Description, service.PricePerLicense)).ToListAsync(cancellationToken);
    }
}
