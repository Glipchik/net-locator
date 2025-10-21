namespace NetLocator.BatchProcessingService.Business.Models;

public class IpProcessingResult
{
    public string IpAddress { get; set; } = null!;
    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public object? IpDetails { get; set; }
}
