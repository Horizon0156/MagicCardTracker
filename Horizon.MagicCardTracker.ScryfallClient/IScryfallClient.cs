using System.Threading;
using System.Threading.Tasks;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public interface IScryfallClient
    {
        Task<CardList> SearchCardsAsync(
            string query, 
            bool includeForeignLanguages, 
            CancellationToken cancellationToken);
    }
}