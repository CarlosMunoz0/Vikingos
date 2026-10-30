namespace Vikingos.Web.Services;

public sealed class VikingoApiException : Exception
{
    public VikingoApiException(string message, bool notFound = false)
        : base(message)
    {
        NotFound = notFound;
    }

    public bool NotFound { get; }
}
