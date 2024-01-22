namespace MagicCardTracker.Pwa.Exceptions;

internal class CardNotFoundException : Exception
{
    public CardNotFoundException()
    {
    }

    public CardNotFoundException(string message) : base(message)
    {
    }

    public CardNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
