using NetLocator.BatchProcessingService.Business.Models;

namespace NetLocator.BatchProcessingService.Business.Interfaces.Services;

public interface IBatchProcessingService
{
    Task<Guid> CreateBatchAsync(List<string> ipAddresses, CancellationToken cancellationToken = default);
    BatchModel? GetBatchStatusAsync(Guid batchId);
    Task ProcessBatchAsync(Guid batchId, CancellationToken cancellationToken = default);
}
