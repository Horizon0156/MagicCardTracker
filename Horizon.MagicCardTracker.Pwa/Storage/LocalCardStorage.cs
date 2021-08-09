using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Horizon.MagicCardTracker.Contracts;
using Horizon.MagicCardTracker.Storage.Abstrations;

namespace Horizon.MagicCardTracker.Pwa.Storage
{
    public class LocalStorageCardLibrary : ICardLibraryPersister
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
            System.Console.WriteLine("Restored library from collection");
            var collection = await _localStorage.GetItemAsync<CollectedCard[]>("cards", cancellationToken);
            
            return collection;
        }
    }
}
