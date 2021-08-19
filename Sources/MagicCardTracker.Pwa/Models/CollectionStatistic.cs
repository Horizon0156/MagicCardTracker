#nullable enable

using System.Collections.Generic;
using System.Linq;
using MagicCardTracker.Contracts;

namespace MagicCardTracker.Pwa.Models
{
    internal class CollectionStatistic
    {
        public CollectionStatistic()
        {
            FiveMostValuableCards = Enumerable.Empty<CollectedCard>();
        }

        public int NumberOfCardsCollected { get; set; }

        public int NumberOfUniqueCardsCollected { get; set; }

        public int NumberOfCommonCards { get; set; }

        public int NumberOfUncommonCards { get; set; }

        public int NumberOfRareCards { get; set; }

        public int NumberOfMythicCards { get; set; }

        public int NumberOfWhiteCards { get; set; }
        
        public int NumberOfBlueCards { get; set; }

        public int NumberOfBlackCards { get; set; }

        public int NumberOfRedCards { get; set; }

        public int NumberOfGreenCards { get; set; }

        public decimal CollectionValueInEuros { get; set; }

        public decimal CollectionValueInDollars { get; set; }

        public IEnumerable<CollectedCard> FiveMostValuableCards { get; set; }
    }
}