#nullable enable

using MagicCardTracker.Contracts;

namespace MagicCardTracker.Pwa.Models
{
    internal class CollectionStatistic
    {
        public int NumberOfCardsCollected { get; set; }

        public int NumberOfUniqueCardsCollected { get; set; }

        public decimal CollectionValueInEuros { get; set; }

        public decimal CollectionValueInDollars { get; set; }

        public CollectedCard? MostValuableCard { get; set; }
    }
}