using System.Text.Json;
using Microsoft.Extensions.Options;
using NetLocator.IPDetailCacheService.External.Dtos;
using NetLocator.IPDetailCacheService.External.Interfaces.ExternalServices;
using NetLocator.IPDetailCacheService.Shared.Configuration;
using NetLocator.IPDetailCacheService.Shared.Exceptions;

namespace NetLocator.IPDetailCacheService.External.ExternalServices;

public class IpLookupExternalService(HttpClient httpClient, IOptions<IpLookupConfiguration> options): IIpLookupExternalService
{
    private readonly IpLookupConfiguration _configuration = options.Value;
    private readonly JsonSerializerOptions _jsonSerializationOptions = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<IpLookupDto> GetDetailsAsync(string ipAddress, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            throw new ArgumentException("IP address cannot be null or empty.", nameof(ipAddress));
        }

        var requestUrl = $"{_configuration.ConnectionString}/ip/{ipAddress}";

        using var response = await httpClient.GetAsync(requestUrl, ct);

        var content = await response.Content.ReadAsStringAsync(ct);
        
        HandleExceptionResponse(content);
        
        var ipStackResponse = JsonSerializer.Deserialize<IpLookupDto>(content, _jsonSerializationOptions);

        return ipStackResponse ?? throw new JsonException("Failed to deserialize the IPStack API response.");
    }

    private void HandleExceptionResponse(string content)
    {
        var errorResponse = JsonSerializer.Deserialize<ExceptionDto>(content, _jsonSerializationOptions);

        if (errorResponse is null)
        {
            throw new JsonException("Failed to deserialize the IPStack API response");
        }

        if (!string.IsNullOrEmpty(errorResponse.Error))
            throw new ExternalIpLookupException($"IP Lookup service responded with an error: {errorResponse.Error}");
    }
}