using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Models;
using MagicCardTracker.Pwa.Queries;
using MagicCardTracker.Storage;
using MediatR;

namespace MagicCardTracker.Pwa.Handlers;

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
            CollectionValue = collection.Sum(c => c.GetCollectionValue(dominatingCurrency)),
            FiveMostValuableCards = collection.OrderByDescending(c => c.GetSingleCardValue(dominatingCurrency))
                                              .Take(5)
        };
    }
}