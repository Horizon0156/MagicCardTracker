namespace MagicCardTracker.Pwa.Settings;

internal abstract class SettingsBase
{
    public string Key => GetType().Name
                                  .Replace("Settings", string.Empty);
}
