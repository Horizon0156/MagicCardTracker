using System.Text.Json;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Commands;
using MagicCardTracker.Pwa.Helpers;
using MagicCardTracker.Storage;
using MediatR;

namespace MagicCardTracker.Pwa.Handlers;

internal class LibraryBackupHandler : 
    IRequestHandler<ExportLibrary>,
    IRequestHandler<ExportCsv>,
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

    public async Task Handle(ImportLibrary request, CancellationToken cancellationToken)
    {
        var collection = await JsonSerializer.DeserializeAsync<CollectedCard[]>(
            request.LibraryBackupStream, 
            cancellationToken: cancellationToken);

        if (collection != null)
        {
            await _cardLibrary.SetCollectionAsync(collection, cancellationToken);    
        }
    }

    public async Task Handle(ExportLibrary request, CancellationToken cancellationToken)
    {
        var collection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);
        var serializedCollection = JsonSerializer.Serialize(collection);

        await _browserTools.SaveAsFileAsync(
            "MCTLibrary.json",
            serializedCollection,
            cancellationToken);
    }

    public async Task Handle(ExportCsv request, CancellationToken cancellationToken)
    {
        var collection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);

        var csvFile = "\"Count\",\"Tradelist Count\",\"Name\",\"Edition\",\"Condition\",\"Language\"," + 
             "\"Foil\",\"Tags\",\"Last Modified\",\"Collector Number\",\"Alter\",\"Proxy\",\"Purchase Price\"";

        foreach(var card in collection)
        {
            var language = string.Equals(card.LanguageCode, "en", StringComparison.OrdinalIgnoreCase)
                ? "English"
                : "German";

            if (card.Count > 0)
            {
                csvFile += Environment.NewLine;
                csvFile += $"\"{card.Count}\",\"{card.Count}\",\"{card.Name}\",\"{card.SetCode}\",\"Near Mint\",\"{language}\"," + 
                    $"\"\",\"\",\"\",\"{card.Number}\",\"False\",\"False\",\"\"";
            }
            if (card.FoilCount > 0)
            {
                csvFile += Environment.NewLine;
                csvFile += $"\"{card.FoilCount}\",\"{card.Count}\",\"{card.Name}\",\"{card.SetCode}\",\"Near Mint\",\"{language}\"," + 
                    $"\"foil\",\"\",\"\",\"{card.Number}\",\"False\",\"False\",\"\"";
            }
        }
        
        await _browserTools.SaveAsFileAsync(
            "MCTExport.csv",
            csvFile,
            cancellationToken);
    }
}
