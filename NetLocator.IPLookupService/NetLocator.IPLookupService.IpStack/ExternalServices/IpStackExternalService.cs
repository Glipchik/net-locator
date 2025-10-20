using System.Text.Json;
using Microsoft.Extensions.Options;
using NetLocator.IPLookupService.IpStack.Dtos;
using NetLocator.IPLookupService.IpStack.Interfaces.ExternalServices;
using NetLocator.IPLookupService.Shared.Configuration;
using NetLocator.IPLookupService.Shared.Exceptions;

namespace NetLocator.IPLookupService.IpStack.ExternalServices;

public class IpStackExternalService(IOptions<IpStackConfiguration> options): IIpStackExternalService
{
    private readonly IpStackConfiguration _configuration = options.Value;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly JsonSerializerOptions _jsonSerializationOptions = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<IpStackAddressDto> GetDetailsAsync(string ipAddress, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            throw new ArgumentException("IP address cannot be null or empty.", nameof(ipAddress));
        }

        var requestUrl = $"{_configuration.ConnectionString}/{ipAddress}" +
                         $"?access_key={Uri.EscapeDataString(_configuration.AccessKey)}";

        var response = await _httpClient.GetAsync(requestUrl, ct);
        
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        
        HandleExceptionResponse(content);
        
        var ipStackResponse = JsonSerializer.Deserialize<IpStackAddressDto>(content, _jsonSerializationOptions);

        return ipStackResponse ?? throw new JsonException("Failed to deserialize the IPStack API response.");
    }

    private void HandleExceptionResponse(string content)
    {
        var errorResponse = JsonSerializer.Deserialize<IpStackErrorDto>(content, _jsonSerializationOptions);

        if (errorResponse is null)
        {
            throw new JsonException("Failed to deserialize the IPStack API response");
        }

        if (!errorResponse.Success && !string.IsNullOrEmpty(errorResponse.Error.Type))
        {
            throw errorResponse.Error.Code switch
            {
                101 => new IPServiceNotAvailableException("Invalid or no access key has been provided"),
                104 => new IPServiceNotAvailableException("Usage limit has been exceeded"),
                _ => new IPServiceNotAvailableException("Unknown issue has happened during request to IP Stack")
            };
        }
    }
}