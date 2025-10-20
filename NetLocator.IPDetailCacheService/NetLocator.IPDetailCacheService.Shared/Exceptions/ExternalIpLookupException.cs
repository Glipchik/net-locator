namespace NetLocator.IPDetailCacheService.Shared.Exceptions;

public class ExternalIpLookupException: Exception
{
    public ExternalIpLookupException()
    {
    }

    public ExternalIpLookupException(string message)
        : base(message)
    {
    }

    public ExternalIpLookupException(string message, Exception inner)
        : base(message, inner)
    {
    }
}