using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace MagicCardTracker.Pwa.Cache
{
    internal class LocalStorageCache : IObjectCache
    {
        private readonly ILocalStorageService _localStorage;

        public LocalStorageCache(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task CacheObject<T>(
            string cacheKey, 
            T @object, 
            CancellationToken cancellationToken)
        {

            await _localStorage.SetItemAsync(cacheKey, @object, cancellationToken);
        }

        public async Task<T> LookupObject<T>(string cacheKey, CancellationToken cancellationToken)
        {
            if (!(await _localStorage.ContainKeyAsync(cacheKey, cancellationToken)))
            {
                throw new CacheMissException();
            }

            var cachedItem = await _localStorage.GetItemAsync<T>(cacheKey, cancellationToken);
            return cachedItem;
        }
    }
}
