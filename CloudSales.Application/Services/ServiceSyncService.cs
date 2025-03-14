using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CloudSales.Application.Services;

public class ServiceSyncService(ICCPApiService ccpApiService, ILogger<ServiceSyncService> logger, IServiceRepository serviceRepository) : IServiceSyncService
{
    public async Task SyncSoftwareServicesAsync(CancellationToken cancellationToken)
    {
        const int batchSize = 1000;
        int currentIndex = 0;

        var softwareServicesFromCCP = await ccpApiService.GetAvailableSoftwareServicesAsync(cancellationToken);
        if (!softwareServicesFromCCP.Any())
        {
            logger.LogWarning("No services returned from CCP.");
            return;
        }

        logger.LogInformation("Syncing {serviceCount} services from CCP.", softwareServicesFromCCP.Count());

        while (true)
        {
            var batch = softwareServicesFromCCP.Skip(currentIndex).Take(batchSize).ToList();

            await ProcessSoftwareServicesBatchAsync(batch, cancellationToken);

            currentIndex += batchSize;

            if (batch.Count < batchSize)
                break;
        }
    }

    private async Task ProcessSoftwareServicesBatchAsync(IEnumerable<SoftwareServiceResponseModel> softwareServicesFromCCP, CancellationToken cancellationToken)
    {
        var existingServices = await serviceRepository.GetAllByExternalIdsAsync(softwareServicesFromCCP.Select(s => s.Id).ToHashSet(), cancellationToken);

        var existingServiceDict = existingServices.ToDictionary(s => s.ExternalId, s => s);

        var servicesToInsert = new List<SoftwareService>();
        var servicesToUpdate = new List<SoftwareService>();
        var servicesToMarkInactive = new List<SoftwareService>();

        foreach (var serviceFromCCP in softwareServicesFromCCP)
        {
            if (existingServiceDict.TryGetValue(serviceFromCCP.Id, out var existingService))
            {
                existingService.Update(serviceFromCCP.Name, serviceFromCCP.Description, serviceFromCCP.PricePerLicense);

                servicesToUpdate.Add(existingService);
                existingServiceDict.Remove(serviceFromCCP.Id);
            }
            else
            {
                var newSoftwareService = new SoftwareService(serviceFromCCP.Id, serviceFromCCP.Name, serviceFromCCP.Description, serviceFromCCP.PricePerLicense);
                servicesToInsert.Add(newSoftwareService);
            }
        }

        servicesToMarkInactive.AddRange(existingServiceDict.Values);

        if (servicesToInsert.Count > 0)
            await serviceRepository.AddRangeAsync(servicesToInsert, cancellationToken);

        if (servicesToMarkInactive.Count > 0)
            foreach (var service in servicesToMarkInactive)
                service.Deactivate();

        if ((servicesToUpdate.Count + servicesToMarkInactive.Count) > 0)
            await serviceRepository.UpdateRangeAsync(servicesToUpdate.Concat(servicesToMarkInactive), cancellationToken);
    }
}
