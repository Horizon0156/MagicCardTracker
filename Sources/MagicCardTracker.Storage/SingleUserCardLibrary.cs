using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MagicCardTracker.Contracts;
using MagicCardTracker.Storage.Abstrations;

namespace MagicCardTracker.Storage
{
    /// <summary>
    ///     Implementation of a card library for a single user.
    /// </summary>
    public class SingleUserCardLibrary : ICardLibrary
    {
        private HashSet<CollectedCard> _collectedCards;

        private bool _isCardLibraryLoaded;

        private readonly ICardLibraryPersister _libraryPersister;

        /// <summary>
        ///     Created a new SingleUserCardLibrary.
        /// </summary>
        /// <param name="libraryPersister"> An instance of a library persister. </param>
        public SingleUserCardLibrary(ICardLibraryPersister libraryPersister)
        {
            _libraryPersister = libraryPersister;
            _collectedCards = new HashSet<CollectedCard>();
        }

        /// <inheritdoc />
        public async Task AddCardAsync(CollectedCard card, CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            _collectedCards.Remove(card);

            if (card.TotalCount > 0)
            {
                _collectedCards.Add(card);
            }
            
            await _libraryPersister.PersistLibraryAsync(_collectedCards, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CollectedCard>> GetCollectedCardsAsync(CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            return _collectedCards;
        }

        /// <inheritdoc />
        public async Task<CollectedCard> SearchInCollectionAsync(Card card, CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            var collectableCard = new CollectedCard(card, 0, 0);

            return _collectedCards.TryGetValue(collectableCard, out var collectedCard)
                ? collectedCard
                : collectableCard;
        }

        /// <inheritdoc />
        public async Task<CollectedCard> SearchInCollectionByIdAsync(string setCode, string cardNumber, string languageCode, CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            return _collectedCards.FirstOrDefault(c => c.SetCode == setCode && c.Number == cardNumber && c.LanguageCode == languageCode);
        }

        /// <inheritdoc />
        private async Task RecoverCardLibraryIfNeeded(CancellationToken cancellationToken)
        {
            if (_isCardLibraryLoaded) 
            {
                return;
            }

            var collectedCards = await _libraryPersister.RestoreLibraryAsync(cancellationToken);
            _collectedCards = collectedCards?.ToHashSet() ?? new HashSet<CollectedCard>();
            _isCardLibraryLoaded = true;
        }

        /// <inheritdoc />
        public Task RestoreCollectionAsync(IEnumerable<CollectedCard> collectedCards, CancellationToken cancellationToken)
        {
            _collectedCards = new HashSet<CollectedCard>(collectedCards);
            return _libraryPersister.PersistLibraryAsync(_collectedCards, cancellationToken);
        }

        /// <inheritdoc />
        public Task ClearCollectionAsync(CancellationToken cancellationToken)
        {
            _collectedCards.Clear();
            return _libraryPersister.PersistLibraryAsync(_collectedCards, cancellationToken);
        }
    }
}
