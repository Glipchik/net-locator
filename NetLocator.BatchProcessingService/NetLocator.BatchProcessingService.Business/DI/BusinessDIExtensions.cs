using Microsoft.Extensions.DependencyInjection;
using NetLocator.BatchProcessingService.Business.Interfaces.Services;
using NetLocator.BatchProcessingService.Business.Services;

namespace NetLocator.BatchProcessingService.Business.DI;

public static class BusinessDIExtensions
{
    public static IServiceCollection AddBusinessDependencies(this IServiceCollection services)
    {
        services.AddScoped<IBatchProcessingService, Services.BatchProcessingService>();
        services.AddScoped<IIpLookupService, Services.IpLookupService>();
        
        return services;
    }
}
