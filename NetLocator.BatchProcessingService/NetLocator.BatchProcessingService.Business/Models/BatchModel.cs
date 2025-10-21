namespace NetLocator.BatchProcessingService.Business.Models;

public class BatchModel
{
    public Guid Id { get; set; }
    public List<string> IpAddresses { get; set; } = [];
    public List<object> IpDetails { get; set; } = [];
    public BatchStatus Status { get; set; }
    public int TotalIpAddresses { get; set; }
    public int ProcessedIpAddresses { get; set; }
    public int SuccessfulIpAddresses { get; set; }
    public int FailedIpAddresses { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<string> Errors { get; set; } = new();
}

public enum BatchStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}
