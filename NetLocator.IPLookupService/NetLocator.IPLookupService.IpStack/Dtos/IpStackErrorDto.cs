namespace NetLocator.IPLookupService.IpStack.Dtos;

public record IpStackErrorDto
{
    public bool Success { get; set; }
    public Error Error { get; set; } = new();

}

public record Error
{
    public int Code { get; set; }
    public string Type { get; set; } = null!;
    public string Info { get; set; } = null!;
}