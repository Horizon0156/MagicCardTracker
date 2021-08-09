
using System.Collections.Generic;
using Horizon.MagicCardTracker.Contracts;

namespace Horizon.MagicCardTracker.Pwa.Models
{
    internal class CardSearchResult
    {
        public bool HasMoreResults { get; set; }

        public int NumberOfMatchedCards { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}