namespace NetLocator.BatchProcessingService.Business.Interfaces.Services;

public interface IIpLookupService
{
    Task<object?> GetIpDetailsAsync(string ipAddress, CancellationToken cancellationToken = default);
}
