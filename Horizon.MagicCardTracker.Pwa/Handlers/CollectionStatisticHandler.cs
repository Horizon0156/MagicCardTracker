#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Horizon.MagicCardTracker.Contracts;
using Horizon.MagicCardTracker.Pwa.Models;
using Horizon.MagicCardTracker.Pwa.Queries;
using Horizon.MagicCardTracker.Storage;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Handlers
{
    internal class CollectionStatisticHandler : IRequestHandler<GetCollectionStatistic, CollectionStatistic>
    {
        private readonly ICardLibrary _cardLibrary;
        
        public CollectionStatisticHandler(ICardLibrary cardLibrary)
        {
            _cardLibrary = cardLibrary;
        }
        public async Task<CollectionStatistic> Handle(GetCollectionStatistic request, CancellationToken cancellationToken)
        {
            var cardCollection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);

            return BuildStatistics(cardCollection);
        }

        private CollectionStatistic BuildStatistics(IEnumerable<CollectedCard>? cardCollection)
        {
            if (cardCollection == null)
            {
                return new CollectionStatistic();
            }

            var collection = cardCollection.ToArray();

            return new CollectionStatistic
            {
                NumberOfCardsCollected = collection.Sum(c => c.TotalCount),
                NumberOfUniqueCardsCollected = collection.Length,
                CollectionValueInEuros = collection.Sum(c => GetTotalCardValueInEuros(c)),
                CollectionValueInDollars = collection.Sum(c => GetTotalCardValueInDollars(c)),
                MostValuableCard = collection.Length > 0 ? collection.Aggregate(
                    (c1, c2) => GetTotalCardValueInDollars(c1) > GetTotalCardValueInDollars(c2) 
                                    ? c1 
                                    : c2) : null
            };
        }

        private decimal GetTotalCardValueInEuros(CollectedCard card)
        {
            if (!card.Prices.HasPricingInformation)
            {
                return 0;
            }

            return card.Count * card.Prices.StandardInEuros ?? 0
                + card.FoilCount * card.Prices.FoiledInEuros ?? 0;
        }

        private decimal GetTotalCardValueInDollars(CollectedCard card)
        {
            if (!card.Prices.HasPricingInformation)
            {
                return 0;
            }

            return card.Count * card.Prices.StandardInDollars ?? 0
                + card.FoilCount * card.Prices.FoiledInDollars ?? 0;
        }
    }
}