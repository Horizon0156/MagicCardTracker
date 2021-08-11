using System.Net.Http;

namespace MagicCardTracker.ScryfallClient
{
    public class ScryfallClientFactory : IScryfallClientFactory
    {
        private readonly IHttpClientFactory _clientFactory;

        public ScryfallClientFactory(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        public ICardsClient Cards => new CardsClient(_clientFactory.CreateClient());

        public ISetsClient Sets => new SetsClient(_clientFactory.CreateClient());

        public ISymbologyClient Symbology => new SymbologyClient(_clientFactory.CreateClient());

        public ICatalogClient Catalog => new CatalogClient(_clientFactory.CreateClient());

        public IRulingsClient Rulings => new RulingsClient(_clientFactory.CreateClient());
    }
}