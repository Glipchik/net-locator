using Microsoft.Extensions.DependencyInjection;
using NetLocator.IPDetailCacheService.External.ExternalServices;
using NetLocator.IPDetailCacheService.External.Interfaces.ExternalServices;

namespace NetLocator.IPDetailCacheService.External.DI;

public static class IpLookupDIExtensions
{   
    public static IServiceCollection AddIpLookupDependencies(this IServiceCollection services)
    {
        services.AddScoped<IIpLookupExternalService, IpLookupExternalService>();
        return services;
    }
}