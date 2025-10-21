namespace NetLocator.BatchProcessingService.Shared.Exceptions;

public class IpAddressInvalidFormatException : Exception
{
    public IpAddressInvalidFormatException(string message) : base(message)
    {
    }
}
