using Microsoft.Extensions.Logging;
using NetLocator.BatchProcessingService.Business.Interfaces.Services;

namespace NetLocator.BatchProcessingService.Business.Services;

public class IpLookupService(
    IHttpClientFactory httpClientFactory,
    ILogger<IpLookupService> logger) : IIpLookupService
{
    public async Task<object?> GetIpDetailsAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient();
            using var response = await httpClient.GetAsync($"ip/{ipAddress}", cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get IP details for {IpAddress}", ipAddress);
            throw;
        }
    }
}
