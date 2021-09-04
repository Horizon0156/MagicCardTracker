#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MagicCardTracker.Contracts;

namespace MagicCardTracker.Storage
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

        /// <summary>
        ///     Gets cards in the collection.
        /// </summary>
        /// <param name="cancellationToken"> A cancellation token. </param>
        /// <returns> Collection of cards. </returns>
        Task<IEnumerable<CollectedCard>> GetCollectedCardsAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Searches the given card in the collection.
        /// </summary>
        /// <param name="card"> The card to search for. </param>
        /// <param name="cancellationToken"> A cancellation token. </param>
        /// <returns> A collectable instance of the card with the actual collected count. </returns>
        Task<CollectedCard> SearchInCollectionAsync(Card card, CancellationToken cancellationToken);

        /// <summary>
        ///     Searches a card in the collection by id. 
        /// </summary>
        /// <param name="setCode"> The set code of the card. </param>
        /// <param name="cardNumber"> The card number. </param>
        /// <param name="languageCode"> The language code. </param>
        /// <param name="cancellationToken"> A cancellation token. </param>
        /// <returns> The card if collected or <c>null</c> if the card was not found. </returns>
        Task<CollectedCard?> SearchInCollectionByIdAsync(string setCode, string cardNumber, string languageCode, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates the collection by the properties of the given cards.
        /// </summary>
        /// <param name="cards"> The collection to merge </param>
        /// <param name="updateMode"> The mode of the collection update. </param>
        /// <param name="cancellationToken"> A cancellation token. </param>
        /// <returns> Operational task. </returns>
        Task UpdatedCollectionAsync(
            IEnumerable<Card> cards,
            UpdateMode updateMode,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Sets the collection to the set of given cards.
        /// </summary>
        /// <param name="collectedCards"> The new collection </param>
        /// <param name="cancellationToken"> A cancellation token. </param>
        /// <returns> Operational task. </returns>
        Task SetCollectionAsync(IEnumerable<CollectedCard> collectedCards, CancellationToken cancellationToken);

        /// <summary>
        ///     Clears the collection. 
        /// </summary>
        /// <param name="cancellationToken"> A cancellation toke. </param>
        /// <returns> Operational task. </returns>
        Task ClearCollectionAsync(CancellationToken cancellationToken);
    }
}
