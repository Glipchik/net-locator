using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NetLocator.IPDetailCacheService.Business.Interfaces.Services;
using NetLocator.IPDetailCacheService.Business.Models;
using NetLocator.IPDetailCacheService.External.Interfaces.ExternalServices;

namespace NetLocator.IPDetailCacheService.Business.Services;

public class IpService(IIpLookupExternalService externalService, IMapper mapper, IMemoryCache memoryCache): IIpService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);
    
    public async Task<IpModel> GetDetailsAsync(string ipAddress, CancellationToken ct)
    {
        memoryCache.TryGetValue<IpModel>(ipAddress, out var modelFromCache);

        if (modelFromCache is not null)
            return mapper.Map<IpModel>(modelFromCache);
            
        var response = await externalService.GetDetailsAsync(ipAddress, ct);
        
        
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration 
        };
        memoryCache.Set(ipAddress, response, cacheEntryOptions);

        return mapper.Map<IpModel>(response);
    }
}