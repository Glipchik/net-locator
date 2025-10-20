using NetLocator.IPLookupService.Business.Models;

namespace NetLocator.IPLookupService.Business.Interfaces.Services;

public interface IIpService
{
    Task<IpModel> GetDetailsAsync(string ipAddress, CancellationToken ct);
}