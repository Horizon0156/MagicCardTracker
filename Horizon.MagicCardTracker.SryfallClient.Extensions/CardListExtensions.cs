using System.Linq;
using Flurl.Http;

namespace Horizon.MagicCardTracker.SrcyfallClient.Extensions
{
    public static class CardListExtensions
    {
        public static Contracts.CardList ToContract(this ScryfallClient.CardList list)
        {
            return new Contracts.CardList
            {
                Cards = list.Cards.Select(c => c.ToContract()),
                TotalNumberOfCards = list.TotalCards,
                LoadMoreAsync = list.HasMoreCards
                    ? async (ct) => (await list.NextPageUrl.GetJsonAsync<ScryfallClient.CardList>(ct)).ToContract()
                    : null
            };
        }
    }
}
