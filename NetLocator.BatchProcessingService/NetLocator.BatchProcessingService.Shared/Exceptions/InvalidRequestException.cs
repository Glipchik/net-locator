namespace NetLocator.BatchProcessingService.Shared.Exceptions;

public class InvalidRequestException : Exception
{
    public InvalidRequestException(string message) : base(message)
    {
    }

    public InvalidRequestException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
