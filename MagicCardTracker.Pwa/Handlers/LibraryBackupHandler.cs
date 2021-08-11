
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Commands;
using MagicCardTracker.Storage;
using MediatR;
using Microsoft.JSInterop;

namespace MagicCardTracker.Pwa.Handlers
{
    internal class LibraryBackupHandler : 
        IRequestHandler<ExportLibraryCommand>,
        IRequestHandler<ImportLibraryCommand>
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ICardLibrary _cardLibrary;

        public LibraryBackupHandler(
            IJSRuntime jsRuntime,
            ICardLibrary cardLibrary)
        {
            _jsRuntime = jsRuntime;
            _cardLibrary = cardLibrary;
        }

        public async Task<Unit> Handle(ImportLibraryCommand request, CancellationToken cancellationToken)
        {
            var collection = await JsonSerializer.DeserializeAsync<CollectedCard[]>(
                request.LibraryBackupStream, 
                cancellationToken: cancellationToken);
            
            await _cardLibrary.RestoreCollectionAsync(collection, cancellationToken);

            return Unit.Value;
        }

        public async Task<Unit> Handle(ExportLibraryCommand request, CancellationToken cancellationToken)
        {
            var collection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);
            var serializedCollection = JsonSerializer.Serialize(collection);

            await _jsRuntime.InvokeAsync<object>(
                "saveFileAs",
                cancellationToken,
                "MCTLibrary.json",
                serializedCollection);

            return Unit.Value;
        }
    }
}
