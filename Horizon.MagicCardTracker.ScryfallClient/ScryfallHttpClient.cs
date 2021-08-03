
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public class ScryfallHttpClient : IScryfallClient
    {
        private const string SCRYFALL_API_URL = "https://api.scryfall.com";
        public Task<CardList> SearchCardsAsync(
            string query, 
            bool includeForeignLanguages, 
            CancellationToken cancellationToken)
        {
            return $"{SCRYFALL_API_URL}/cards/search?q={query}&include_multilingual={includeForeignLanguages}"
                    .GetJsonAsync<CardList>();
        }
    }
}