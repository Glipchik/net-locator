using Microsoft.Extensions.DependencyInjection;
using NetLocator.IPLookupService.IpStack.ExternalServices;
using NetLocator.IPLookupService.IpStack.Interfaces.ExternalServices;

namespace NetLocator.IPLookupService.IpStack.DI;

public static class IpStackDIExtensions
{   
    public static IServiceCollection AddIpStackDependencies(this IServiceCollection services)
    {
        services.AddScoped<IIpStackExternalService, IpStackExternalService>();
        return services;
    }
}