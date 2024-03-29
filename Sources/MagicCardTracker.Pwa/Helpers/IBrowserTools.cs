namespace MagicCardTracker.Pwa.Helpers;

internal interface IBrowserTools
{
    Task ScrollToTopAsync(CancellationToken cancellationToken);

    Task SaveAsFileAsync(
        string filename, 
        string fileContent, 
        CancellationToken cancellationToken);
}
