using NetLocator.IPLookupService.IpStack.Dtos;

namespace NetLocator.IPLookupService.IpStack.Interfaces.ExternalServices;

public interface IIpStackExternalService
{
    Task<IpStackAddressDto> GetDetailsAsync(string ipAddress, CancellationToken ct);
}