
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public class ScryfallHttpClient : IScryfallClient
    {
        private const string SCRYFALL_API_URL = "https://api.scryfall.com";

        public Task<Card> GetCardByNumberAsync(string setCode, string cardNumber, string languageCode, CancellationToken cancellationToken)
        {
            return $"{SCRYFALL_API_URL}/cards/{setCode.ToLower()}/{cardNumber.ToLower()}/{languageCode?.ToLower() ?? "en"}"
                    .GetJsonAsync<Card>();
        }

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