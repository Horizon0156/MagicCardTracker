using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Horizon.MagicCardTracker.Contracts;

namespace Horizon.MagicCardTracker.Storage
{
    public interface ICardLibrary
    {
        /// <summary>
        ///     Updates the collected card in the library.
        ///     In case the card is already in the library, the count will be updated.
        /// </summary>
        /// <param name="card"> The card which should be tracked. </param>
        /// <param name="cancellationToken"> A cancellation token. </param>
        /// <returns> An operational task. </returns>
        Task AddCardAsync(CollectedCard card, CancellationToken cancellationToken);

        Task<IEnumerable<CollectedCard>> GetCollectedCardsAsync(CancellationToken cancellationToken);

        Task<CollectedCard> SearchInCollectionAsync(Card card, CancellationToken cancellationToken);

        Task<CollectedCard> SearchInCollectionByIdAsync(string setCode, string cardNumber, string languageCode, CancellationToken cancellationToken);

        Task RestoreCollectionAsync(IEnumerable<CollectedCard> collectedCards, CancellationToken cancellationToken);

        Task ClearCollectionAsync(CancellationToken cancellationToken);
    }
}
