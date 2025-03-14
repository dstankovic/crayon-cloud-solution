using CloudSales.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudSales.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class SyncServicesBackgroundJob(IServiceSyncService serviceSyncService, ILogger<SyncServicesBackgroundJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Started processing {UtcNow}", DateTime.UtcNow);

        await serviceSyncService.SyncSoftwareServicesAsync(CancellationToken.None);
    }
}
