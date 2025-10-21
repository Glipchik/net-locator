using NetLocator.BatchProcessingService.Business.Models;

namespace NetLocator.BatchProcessingService.Business.Interfaces.Services;

public interface IBatchProcessingService
{
    Task<Guid> CreateBatchAsync(List<string> ipAddresses, CancellationToken cancellationToken = default);
    Task<BatchModel?> GetBatchStatusAsync(Guid batchId, CancellationToken cancellationToken = default);
    Task ProcessBatchAsync(Guid batchId, CancellationToken cancellationToken = default);
}
