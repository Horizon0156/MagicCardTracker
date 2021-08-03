using System.Net.Http;

namespace MagicCardTracker.ScryfallClient
{
    /// <summary>
    ///     Implementation of a IScryfallClientFactory that will construct
    ///     proper generated client services using a IHttpClientFactory.
    /// </summary>
    public class ScryfallClientFactory : IScryfallClientFactory
    {
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        ///     Creates a new ScryfallClientFactory
        /// </summary>
        /// <param name="clientFactory"> An instance of a http client factory </param>
        public ScryfallClientFactory(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        /// <inheritdoc />
        public ICardsClient Cards => new CardsClient(_clientFactory.CreateClient());

        /// <inheritdoc />
        public ISetsClient Sets => new SetsClient(_clientFactory.CreateClient());

        /// <inheritdoc />
        public ISymbologyClient Symbology => new SymbologyClient(_clientFactory.CreateClient());

        /// <inheritdoc />
        public ICatalogClient Catalog => new CatalogClient(_clientFactory.CreateClient());

        /// <inheritdoc />
        public IRulingsClient Rulings => new RulingsClient(_clientFactory.CreateClient());
    }
}