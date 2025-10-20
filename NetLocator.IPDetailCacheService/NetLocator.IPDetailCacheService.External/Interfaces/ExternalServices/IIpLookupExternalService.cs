using NetLocator.IPDetailCacheService.External.Dtos;

namespace NetLocator.IPDetailCacheService.External.Interfaces.ExternalServices;

public interface IIpLookupExternalService
{
    Task<IpLookupDto> GetDetailsAsync(string ipAddress, CancellationToken ct);
}