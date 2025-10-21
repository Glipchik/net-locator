namespace NetLocator.BatchProcessingService.API.Dtos;

public record BatchStatusDto
{
    public Guid BatchId { get; set; }
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
