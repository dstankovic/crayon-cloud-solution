namespace CloudSales.Application.Interfaces;

public interface IServiceSyncService
{
    Task SyncSoftwareServicesAsync(CancellationToken cancellationToken);
}
