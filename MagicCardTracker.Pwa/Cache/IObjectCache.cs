using System.Threading;
using System.Threading.Tasks;

namespace MagicCardTracker.Pwa.Cache
{
    internal interface IObjectCache
    {
        Task CacheObject<T>(string cacheKey, T @object, CancellationToken cancellationToken);

        Task<T> LookupObject<T>(string cacheKey, CancellationToken cancellationToken);
    }
}
