namespace NetLocator.BatchProcessingService.Shared.Exceptions;

public class InvalidRequestException : Exception
{
    public InvalidRequestException(string message) : base(message)
    {
    }
}
