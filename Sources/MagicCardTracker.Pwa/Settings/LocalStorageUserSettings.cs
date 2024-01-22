using Blazored.LocalStorage;
using MagicCardTracker.Contracts;

namespace MagicCardTracker.Pwa.Settings;

internal class LocalStorageUserSettings : IUserSettings
{
    private readonly ILocalStorageService _localStorage;

    public Currency Currency { get; set; }

    public LocalStorageUserSettings(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task LoadSettingsAsync(CancellationToken cancellationToken)
    {
        Currency = await _localStorage.GetItemAsync<Currency>(
            nameof(Currency).ToLower(),
            cancellationToken);
    }

    public async Task SaveSettingsAsync(CancellationToken cancellationToken)
    {
        await _localStorage.SetItemAsync(
            nameof(Currency).ToLower(),
            Currency,
            cancellationToken);
    }
}
