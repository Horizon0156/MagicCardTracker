namespace MagicCardTracker.ScryfallClient
{
    /// <summary>
    ///     Interface for a factory that delivers instances of Scryfall clients.
    /// </summary>
    public interface IScryfallClientFactory
    {
        /// <summary>
        ///     Gets an instance if the Cards API client
        /// </summary>
        ICardsClient Cards { get; }

        /// <summary>
        ///     Gets an instance if the Sets API client
        /// </summary>
        ISetsClient Sets { get; }

        /// <summary>
        ///     Gets an instance if the Symbology API client
        /// </summary>
        ISymbologyClient Symbology { get; }

        /// <summary>
        ///     Gets an instance if the Catalog API client
        /// </summary>
        ICatalogClient Catalog { get; }

        /// <summary>
        ///     Gets an instance if the Rulings API client
        /// </summary>
        IRulingsClient Rulings { get; }
    }
}