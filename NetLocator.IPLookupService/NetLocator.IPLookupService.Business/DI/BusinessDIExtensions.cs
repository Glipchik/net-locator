using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetLocator.IPLookupService.Business.Interfaces;
using NetLocator.IPLookupService.Business.Services;
using NetLocator.IPLookupService.IpStack.DI;

namespace NetLocator.IPLookupService.Business.DI;

public static class BusinessDIExtensions
{
    public static IServiceCollection AddBusinessDependencies(this IServiceCollection services)
    {
        services.AddIpStackDependencies();

        services.AddScoped<IIpService, IpService>();
        return services;
    }
}