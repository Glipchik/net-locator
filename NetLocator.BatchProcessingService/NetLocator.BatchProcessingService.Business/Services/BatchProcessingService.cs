using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetLocator.BatchProcessingService.Business.Interfaces.Services;
using NetLocator.BatchProcessingService.Business.Models;

namespace NetLocator.BatchProcessingService.Business.Services;

public class BatchProcessingService(
    IIpLookupService ipLookupService,
    IMemoryCache memoryCache,
    ILogger<BatchProcessingService> logger) : IBatchProcessingService
{
    private const int ChunkSize = 10;
    private static readonly TimeSpan BatchCacheDuration = TimeSpan.FromHours(24);

    public async Task<Guid> CreateBatchAsync(List<string> ipAddresses, CancellationToken cancellationToken = default)
    {
        var batchId = Guid.NewGuid();
        var batch = new BatchModel
        {
            Id = batchId,
            IpAddresses = ipAddresses,
            Status = BatchStatus.Pending,
            TotalIpAddresses = ipAddresses.Count,
            CreatedAt = DateTime.UtcNow
        };

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = BatchCacheDuration
        };
        memoryCache.Set(batchId, batch, cacheEntryOptions);

        logger.LogInformation("Created batch {BatchId} with {Count} IP addresses", batchId, ipAddresses?.Count ?? 0);

        _ = Task.Run(async () => await ProcessBatchAsync(batchId, cancellationToken), cancellationToken);

        return batchId;
    }

    public BatchModel? GetBatchStatusAsync(Guid batchId)
    {
        memoryCache.TryGetValue<BatchModel>(batchId, out var batch);
        return batch;
    }

    public async Task ProcessBatchAsync(Guid batchId, CancellationToken cancellationToken = default)
    {
        if (!memoryCache.TryGetValue<BatchModel>(batchId, out var batch))
        {
            logger.LogWarning("Batch {BatchId} not found", batchId);
            return;
        }

        batch!.Status = BatchStatus.Processing;
        memoryCache.Set(batchId, batch);

        logger.LogInformation("Starting processing for batch {BatchId}", batchId);

        try
        {
            var ipAddresses = batch.IpAddresses;
            var chunks = ipAddresses.Chunk(ChunkSize).ToList();

            foreach (var chunk in chunks)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await ProcessChunkAsync(batchId, chunk.ToList(), cancellationToken);
            }

            batch.Status = BatchStatus.Completed;
            batch.CompletedAt = DateTime.UtcNow;
            memoryCache.Set(batchId, batch);

            logger.LogInformation("Completed processing for batch {BatchId}. Processed: {Processed}, Successful: {Successful}, Failed: {Failed}",
                batchId, batch?.ProcessedIpAddresses ?? 0, batch?.SuccessfulIpAddresses ?? 0, batch?.FailedIpAddresses ?? 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing batch {BatchId}", batchId);
            batch.Status = BatchStatus.Failed;
            batch.CompletedAt = DateTime.UtcNow;
            batch.Errors.Add($"Processing failed: {ex.Message}");
            memoryCache.Set(batchId, batch);
        }
    }

    private async Task ProcessChunkAsync(Guid batchId, List<string> chunk, CancellationToken cancellationToken)
    {
        if (!memoryCache.TryGetValue<BatchModel>(batchId, out var batch))
            return;

        var tasks = chunk.Select(async ipAddress =>
        {
            try
            {
                var ipDetails = await ipLookupService.GetIpDetailsAsync(ipAddress, cancellationToken);
                
                var cacheKey = $"ip_details_{ipAddress}";
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
                };
                memoryCache.Set(cacheKey, ipDetails, cacheEntryOptions);

                return new IpProcessingResult
                {
                    IpAddress = ipAddress,
                    IsSuccessful = true,
                    IpDetails = ipDetails
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process IP address {IpAddress} in batch {BatchId}", ipAddress, batchId);
                return new IpProcessingResult
                {
                    IpAddress = ipAddress,
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };
            }
        });

        var results = await Task.WhenAll(tasks);

        batch!.ProcessedIpAddresses += chunk.Count;
        batch.SuccessfulIpAddresses += results.Count(r => r.IsSuccessful);
        batch.FailedIpAddresses += results.Count(r => !r.IsSuccessful);
        batch.IpDetails.Add(results.Select(r => r.IpDetails));

        foreach (var result in results.Where(r => !r.IsSuccessful))
        {
            batch.Errors.Add($"IP {result.IpAddress}: {result.ErrorMessage}");
        }

        memoryCache.Set(batchId, batch);
    }
}
