using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetLocator.IPDetailCacheService.Business.Interfaces.Services;
using NetLocator.IPDetailCacheService.Business.Models;
using NetLocator.IPDetailCacheService.External.Interfaces.ExternalServices;
using NetLocator.IPDetailCacheService.Shared.Configuration;

namespace NetLocator.IPDetailCacheService.Business.Services;

public class IpService(IIpLookupExternalService externalService, IMapper mapper, IMemoryCache memoryCache, 
    IOptions<CacheConfiguration> cacheConfiguration): IIpService
{
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(cacheConfiguration.Value.DurationInMinutes);
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();
    
    public async Task<IpModel> GetDetailsAsync(string ipAddress, CancellationToken ct)
    {
        if (memoryCache.TryGetValue<IpModel>(ipAddress, out var modelFromCache))
        {
            return modelFromCache!;
        }

        var semaphore = _semaphores.GetOrAdd(ipAddress, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(ct);
        try
        {
            if (memoryCache.TryGetValue<IpModel>(ipAddress, out var cachedModel))
            {
                return cachedModel!;
            }

            var response = await externalService.GetDetailsAsync(ipAddress, ct);
            
            var ipModel = mapper.Map<IpModel>(response);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration 
            };
            memoryCache.Set(ipAddress, ipModel, cacheEntryOptions);

            return ipModel;
        }
        finally
        {
            semaphore.Release();
            
            if (semaphore.CurrentCount == 1)
            {
                _semaphores.TryRemove(ipAddress, out _);
                semaphore.Dispose();
            }
        }
    }
}