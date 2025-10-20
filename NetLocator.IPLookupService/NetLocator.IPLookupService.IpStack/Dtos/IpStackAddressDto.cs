using System.Text.Json.Serialization;

namespace NetLocator.IPLookupService.IpStack.Dtos;

public record IpStackAddressDto
{
    public string Ip { get; set; } = null!;
    
    public string Type { get; set; } = null!;

    [JsonPropertyName("continent_name")]
    public string ContinentName { get; set; } = null!;

    [JsonPropertyName("country_name")]
    public string CountryName { get; set; } = null!;

    [JsonPropertyName("region_name")]
    public string RegionName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}