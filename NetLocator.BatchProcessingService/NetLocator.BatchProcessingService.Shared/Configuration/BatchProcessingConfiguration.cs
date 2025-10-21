namespace NetLocator.BatchProcessingService.Shared.Configuration;

public class BatchProcessingConfiguration
{
    public string IpLookupServiceUrl { get; set; } = null!;
    public int ChunkSize { get; set; } = 10;
    public int BatchCacheDurationHours { get; set; } = 24;
    public int IpDetailsCacheDurationMinutes { get; set; } = 60;
}
