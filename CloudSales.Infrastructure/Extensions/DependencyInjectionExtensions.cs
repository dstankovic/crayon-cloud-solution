using CloudSales.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CloudSales.Application.Interfaces;
using CloudSales.Infrastructure.Services;
using CloudSales.Infrastructure.Persistence.Interceptors;
using Quartz;
using CloudSales.Infrastructure.Jobs;

namespace CloudSales.Infrastructure.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>((sp, options) => options
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(sp.GetService<UpdateTimestampInterceptor>()!)
            .UseLazyLoadingProxies());

        services.TryAddTransient<UpdateTimestampInterceptor>();
        services.TryAddTransient<IAccountRepository, AccountRepository>();
        services.TryAddTransient<IServiceRepository, ServiceRepository>();
        services.TryAddTransient<ISubscriptionRepository, SubscriptionRepository>();
        services.TryAddTransient<ICCPApiService, CCPApiService>();

        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = JobKey.Create(nameof(SyncServicesBackgroundJob));
            var frequency = int.TryParse(configuration["CronJobs:ServiceSyncServiceIntervalMinutes"], out var value) ? value : 30;

            options
                .AddJob<SyncServicesBackgroundJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                                              .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(frequency).RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
