
namespace MagicCardTracker.Pwa.Settings
{
    internal class SettingsBase
    {
        public string Key => GetType().Name
                                      .Replace("Settings", string.Empty);
    }
}
