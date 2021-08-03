using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Horizon.MagicCardTracker.Contracts;

namespace Horizon.MargicCardTracker.Storage.Abstrations
{
    /// <summary>
    ///     Definition for a client specific single user storage persistence.
    /// </summary>
    public interface ICardLibraryPersister
    {
        /// <summary>
        ///     Persists the given library asnychronous.
        /// </summary>
        /// <param name="collection"> The collection which should be persisted. </param>
        /// <param name="cancellationToken"> A cancellation toke. </param>
        /// <returns> Operational task. </returns>
        Task PersistLibraryAsync(
            IEnumerable<CollectedCard> collection, 
            CancellationToken cancellationToken);

        /// <summary>
        ///     Restores a persisted library asnychronous.
        /// </summary>
        /// <param name="cancellationToken"> A cancellation toke. </param>
        /// <returns> The restored library of collected cards. </returns>
        Task<IEnumerable<CollectedCard>> RestoreLibraryAsync(
            CancellationToken cancellationToken);
    }
}
