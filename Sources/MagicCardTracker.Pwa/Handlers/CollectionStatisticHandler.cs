#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Models;
using MagicCardTracker.Pwa.Queries;
using MagicCardTracker.Storage;
using MediatR;

namespace MagicCardTracker.Pwa.Handlers
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

            return BuildStatistics(cardCollection, request.DominatingCurrency);
        }

        private CollectionStatistic BuildStatistics(
            IEnumerable<CollectedCard>? cardCollection,
            Currency dominatingCurrency)
        {
            if (cardCollection == null)
            {
                return new CollectionStatistic();
            }

            var collection = cardCollection.ToArray();
            var collectionByRarity = collection.GroupBy(c => c.Rarity);

            return new CollectionStatistic
            {
                NumberOfCardsCollected = collection.Sum(c => c.TotalCount),
                NumberOfUniqueCardsCollected = collection.Length,
                NumberOfCommonCards = collectionByRarity.FirstOrDefault(
                    g => g.Key == CardRarity.Common)?.Count() ?? 0,
                NumberOfUncommonCards = collectionByRarity.FirstOrDefault(
                    g => g.Key == CardRarity.Uncommon)?.Count() ?? 0,
                NumberOfRareCards = collectionByRarity.FirstOrDefault(
                    g => g.Key == CardRarity.Rare)?.Count() ?? 0,
                NumberOfMythicCards = collectionByRarity.FirstOrDefault(
                    g => g.Key == CardRarity.Mythic)?.Count() ?? 0,
                NumberOfWhiteCards = collection.Where(c => c.Colors.Contains("W")).Count(),
                NumberOfBlueCards = collection.Where(c => c.Colors.Contains("U")).Count(),
                NumberOfBlackCards = collection.Where(c => c.Colors.Contains("B")).Count(),
                NumberOfRedCards = collection.Where(c => c.Colors.Contains("R")).Count(),
                NumberOfGreenCards = collection.Where(c => c.Colors.Contains("G")).Count(),
                CollectionValueInEuros = collection.Sum(c => GetTotalCardValue(c, Currency.Euro)),
                CollectionValueInDollars = collection.Sum(c => GetTotalCardValue(c, Currency.Dollar)),
                FiveMostValuableCards = collection.OrderByDescending(c => GetSingleCardValue(c, dominatingCurrency))
                                                  .Take(5)
            };
        }

        private decimal GetTotalCardValue(CollectedCard card, Currency currency)
        {
            if (!card.Prices.HasPricingInformation)
            {
                return 0;
            }

            return currency == Currency.Euro
                ? card.Count * card.Prices.StandardInEuros ?? 0
                    + card.FoilCount * card.Prices.FoiledInEuros ?? 0
                : card.Count * card.Prices.StandardInDollars ?? 0
                    + card.FoilCount * card.Prices.FoiledInDollars ?? 0;
        }

        private decimal GetSingleCardValue(CollectedCard card, Currency currency)
        {
            if (!card.Prices.HasPricingInformation)
            {
                return 0;
            }

            var standardValue = card.Count > 0
                ? (currency == Currency.Dollar
                    ? card.Prices.StandardInDollars
                    : card.Prices.StandardInEuros) ?? 0
                : 0;

            var foilValue = card.FoilCount > 0
                ? (currency == Currency.Dollar
                    ? card.Prices.FoiledInDollars
                    : card.Prices.FoiledInEuros) ?? 0
                : 0;

            return Math.Max(standardValue, foilValue);
        }
    }
}