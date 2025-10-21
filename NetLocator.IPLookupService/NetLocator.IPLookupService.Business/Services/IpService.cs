using AutoMapper;
using NetLocator.IPLookupService.Business.Interfaces.Services;
using NetLocator.IPLookupService.Business.Models;
using NetLocator.IPLookupService.IpStack.Interfaces.ExternalServices;

namespace NetLocator.IPLookupService.Business.Services;

public class IpService(IIpStackExternalService externalService, IMapper mapper): IIpService
{
    public async Task<IpModel> GetDetailsAsync(string ipAddress, CancellationToken ct)
    {
        var response = await externalService.GetDetailsAsync(ipAddress, ct);

        return mapper.Map<IpModel>(response);
    }
}