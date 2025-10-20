using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetLocator.IPDetailCacheService.API.Dtos;
using NetLocator.IPDetailCacheService.API.Validators;
using NetLocator.IPDetailCacheService.Business.Interfaces.Services;
using NetLocator.IPDetailCacheService.Shared.Exceptions;

namespace NetLocator.IPDetailCacheService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class IpController(IIpService ipService, IMapper mapper) : ControllerBase
{
    [HttpGet("{ipAddress}")]
    public async Task<IpDto> Get([FromRoute] string ipAddress, CancellationToken ct)
    {
        var isAddressValid = IpValidator.Validate(ipAddress);

        if (!isAddressValid)
            throw new IpAddressInvalidFormatException("Provided IP address is in an invalid format");

        var result = await ipService.GetDetailsAsync(ipAddress, ct);

        return mapper.Map<IpDto>(result);
    }
}