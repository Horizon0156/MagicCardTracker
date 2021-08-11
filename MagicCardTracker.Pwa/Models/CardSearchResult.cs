
using System.Collections.Generic;
using MagicCardTracker.Contracts;

namespace MagicCardTracker.Pwa.Models
{
    internal class CardSearchResult
    {
        public bool HasMoreResults { get; set; }

        public int NumberOfMatchedCards { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}