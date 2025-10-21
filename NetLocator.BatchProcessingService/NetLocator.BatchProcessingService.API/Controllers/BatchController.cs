using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetLocator.BatchProcessingService.API.Dtos;
using NetLocator.BatchProcessingService.API.Validators;
using NetLocator.BatchProcessingService.Business.Interfaces.Services;
using NetLocator.BatchProcessingService.Business.Models;
using NetLocator.BatchProcessingService.Shared.Exceptions;

namespace NetLocator.BatchProcessingService.API.Controllers;

[ApiController]
[Route("ip/[controller]")]
public class BatchController(IBatchProcessingService batchProcessingService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<BatchResponseDto> CreateBatch([FromBody] BatchRequestDto request, CancellationToken ct)
    {
        if (request.IpAddresses == null || !request.IpAddresses.Any())
        {
            throw new InvalidRequestException("IP addresses list cannot be empty");
        }

        // Validate all IP addresses
        foreach (var ipAddress in request.IpAddresses)
        {
            if (!IpValidator.Validate(ipAddress))
            {
                throw new IpAddressInvalidFormatException($"Invalid IP address format: {ipAddress}");
            }
        }

        var batchId = await batchProcessingService.CreateBatchAsync(request.IpAddresses, ct);
        var batch = await batchProcessingService.GetBatchStatusAsync(batchId, ct);

        return mapper.Map<BatchResponseDto>(batch!);
    }

    [HttpGet("{batchId}")]
    public async Task<BatchStatusDto> GetBatchStatus([FromRoute] Guid batchId, CancellationToken ct)
    {
        var batch = await batchProcessingService.GetBatchStatusAsync(batchId, ct);
        
        if (batch is null)
        {
            throw new BatchNotFoundException($"Batch with ID {batchId} not found");
        }

        return mapper.Map<BatchStatusDto>(batch);
    }
}
