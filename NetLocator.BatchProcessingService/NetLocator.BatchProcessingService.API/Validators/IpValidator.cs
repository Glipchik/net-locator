using System.Net;

namespace NetLocator.BatchProcessingService.API.Validators;

internal static class IpValidator
{
    internal static bool Validate(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }
}
