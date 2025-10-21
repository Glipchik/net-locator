namespace NetLocator.BatchProcessingService.API.Dtos;

public record BatchRequestDto
{
    public List<string> IpAddresses { get; set; } = new();
}
