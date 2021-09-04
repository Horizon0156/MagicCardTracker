using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MagicCardTracker.Contracts;
using MagicCardTracker.Storage.Abstrations;

namespace MagicCardTracker.Pwa.Storage
{
    internal class LocalStorageCardLibrary : ICardLibraryPersister
    {
        private readonly ILocalStorageService _localStorage;

        public LocalStorageCardLibrary(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        
        public async Task PersistLibraryAsync(IEnumerable<CollectedCard> collection, CancellationToken cancellationToken)
        {
            await _localStorage.SetItemAsync("cards", collection.ToArray(), cancellationToken);
        }

        public async Task<IEnumerable<CollectedCard>> RestoreLibraryAsync(CancellationToken cancellationToken)
        {
            var collection = await _localStorage.GetItemAsync<CollectedCard[]>("cards", cancellationToken);
            
            return collection;
        }
    }
}
