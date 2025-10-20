namespace NetLocator.IPDetailCacheService.Business.Models;

public class IpModel
{
    public string Ip { get; set; } = null!;
    
    public string Type { get; set; } = null!;

    public string ContinentName { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public string RegionName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}