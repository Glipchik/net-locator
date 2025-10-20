using NetLocator.IPLookupService.Business.Models;

namespace NetLocator.IPLookupService.Business.Interfaces;

public interface IIpService
{
    Task<IpModel> GetDetailsAsync(string ipAddress, CancellationToken ct);
}