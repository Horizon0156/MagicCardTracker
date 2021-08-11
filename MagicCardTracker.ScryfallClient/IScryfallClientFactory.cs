namespace MagicCardTracker.ScryfallClient
{
    public interface IScryfallClientFactory
    {
        ICardsClient Cards { get; }

        ISetsClient Sets { get; }

        ISymbologyClient Symbology { get; }

        ICatalogClient Catalog { get; }

        IRulingsClient Rulings { get; }
    }
}