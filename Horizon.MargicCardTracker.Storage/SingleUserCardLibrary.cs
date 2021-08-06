using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Horizon.MagicCardTracker.Contracts;
using Horizon.MargicCardTracker.Storage.Abstrations;

namespace Horizon.MargicCardTracker.Storage
{
    public class SingleUserCardLibrary : ICardLibrary
    {
        private HashSet<CollectedCard> _collectedCards;

        private bool _isCardLibraryLoaded;

        private readonly ICardLibraryPersister _libraryPersister;

        public SingleUserCardLibrary(ICardLibraryPersister libraryPersister)
        {
            _libraryPersister = libraryPersister;
            _collectedCards = new HashSet<CollectedCard>();
        }
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

        public async Task<IEnumerable<CollectedCard>> GetCollectedCardsAsync(CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            return _collectedCards;
        }

        public async Task<CollectedCard> SearchInCollectionAsync(Card card, CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            var collectableCard = new CollectedCard(card, 0, 0);

            return _collectedCards.TryGetValue(collectableCard, out var collectedCard)
                ? collectedCard
                : collectableCard;
        }

        public async Task<CollectedCard> SearchInCollectionByIdAsync(string setCode, string cardNumber, string languageCode, CancellationToken cancellationToken)
        {
            await RecoverCardLibraryIfNeeded(cancellationToken);

            return _collectedCards.FirstOrDefault(c => c.SetCode == setCode && c.Number == cardNumber && c.LanguageCode == languageCode);
        }

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

        public Task RestoreCollectionAsync(IEnumerable<CollectedCard> collectedCards, CancellationToken cancellationToken)
        {
            _collectedCards = new HashSet<CollectedCard>(collectedCards);
            return _libraryPersister.PersistLibraryAsync(_collectedCards, cancellationToken);
        }

        public Task ClearCollectionAsync(CancellationToken cancellationToken)
        {
            _collectedCards.Clear();
            return _libraryPersister.PersistLibraryAsync(_collectedCards, cancellationToken);
        }
    }
}
