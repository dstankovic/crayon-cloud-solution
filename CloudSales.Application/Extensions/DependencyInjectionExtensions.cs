using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CloudSales.Application.Interfaces;
using CloudSales.Application.Services;

namespace CloudSales.Infrastructure.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddTransient<ISubscriptionService, SubscriptionService>();

        return services;
    }
}
