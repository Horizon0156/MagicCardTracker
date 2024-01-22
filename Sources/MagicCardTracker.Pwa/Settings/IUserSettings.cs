using MagicCardTracker.Contracts;

namespace MagicCardTracker.Pwa.Settings;

internal interface IUserSettings
{
    Currency Currency { get; set; }

    Task LoadSettingsAsync(CancellationToken cancellationToken);
    
    Task SaveSettingsAsync(CancellationToken cancellationToken);
}
