using CloudSales.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CloudSales.Application.Interfaces;

namespace CloudSales.Infrastructure.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>((sp, options) => options
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLazyLoadingProxies());

        services.TryAddTransient<IAccountRepository, AccountRepository>();
        services.TryAddTransient<IServiceRepository, ServiceRepository>();
        services.TryAddTransient<ISubscriptionRepository, SubscriptionRepository>();

        return services;
    }
}
