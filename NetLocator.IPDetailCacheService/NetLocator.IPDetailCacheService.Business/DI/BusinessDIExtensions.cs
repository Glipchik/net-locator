using Microsoft.Extensions.DependencyInjection;
using NetLocator.IPDetailCacheService.Business.Interfaces.Services;
using NetLocator.IPDetailCacheService.Business.Services;
using NetLocator.IPDetailCacheService.External.DI;

namespace NetLocator.IPDetailCacheService.Business.DI;

public static class BusinessDIExtensions
{
    public static IServiceCollection AddBusinessDependencies(this IServiceCollection services)
    {
        services.AddIpLookupDependencies();

        services.AddScoped<IIpService, IpService>();
        return services;
    }
}