
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Commands;
using MagicCardTracker.Pwa.Helpers;
using MagicCardTracker.Storage;
using MediatR;

namespace MagicCardTracker.Pwa.Handlers
{
    internal class LibraryBackupHandler : 
        IRequestHandler<ExportLibrary>,
        IRequestHandler<ImportLibrary>
    {
        private readonly IBrowserTools _browserTools;
        private readonly ICardLibrary _cardLibrary;

        public LibraryBackupHandler(
            IBrowserTools browserTools,
            ICardLibrary cardLibrary)
        {
            _browserTools = browserTools;
            _cardLibrary = cardLibrary;
        }

        public async Task<Unit> Handle(ImportLibrary request, CancellationToken cancellationToken)
        {
            var collection = await JsonSerializer.DeserializeAsync<CollectedCard[]>(
                request.LibraryBackupStream, 
                cancellationToken: cancellationToken);
            
            await _cardLibrary.RestoreCollectionAsync(collection, cancellationToken);

            return Unit.Value;
        }

        public async Task<Unit> Handle(ExportLibrary request, CancellationToken cancellationToken)
        {
            var collection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);
            var serializedCollection = JsonSerializer.Serialize(collection);

            await _browserTools.SaveAsFileAsync(
                "MCTLibrary.json",
                serializedCollection,
                cancellationToken);

            return Unit.Value;
        }
    }
}
