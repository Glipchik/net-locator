namespace NetLocator.BatchProcessingService.API.Dtos;

public record BatchResponseDto
{
    public Guid BatchId { get; set; }
    public int TotalIpAddresses { get; set; }
    public DateTime CreatedAt { get; set; }
}
