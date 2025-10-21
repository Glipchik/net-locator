namespace NetLocator.BatchProcessingService.Shared.Exceptions;

public class BatchNotFoundException : Exception
{
    public BatchNotFoundException(string message) : base(message)
    {
    }
}
