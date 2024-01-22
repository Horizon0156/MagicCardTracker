namespace MagicCardTracker.Pwa.Cache;

internal class CacheMissException : Exception
{
    public CacheMissException()
    {
    }

    public CacheMissException(string message) : base(message)
    {
    }

    public CacheMissException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
