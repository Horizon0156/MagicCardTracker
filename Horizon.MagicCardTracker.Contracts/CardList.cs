using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Horizon.MagicCardTracker.Contracts
{
    public class CardList
    {
        public int TotalNumberOfCards { get; set; }

        public IEnumerable<Card> Cards { get; set; }

        public bool HasMoreCardsToLoad => LoadMoreAsync != null;

        public Func<CancellationToken, Task<CardList>> LoadMoreAsync { get; set; }
    }
}
