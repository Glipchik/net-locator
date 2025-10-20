using NetLocator.IPDetailCacheService.Business.Models;

namespace NetLocator.IPDetailCacheService.Business.Interfaces.Services;

public interface IIpService
{
    Task<IpModel> GetDetailsAsync(string ipAddress, CancellationToken ct);
}