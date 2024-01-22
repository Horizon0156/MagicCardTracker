namespace MagicCardTracker.Pwa.Settings;

internal class CollectionSettings : SettingsBase
{   
    public int DisplayBatchSize { get; set; }

    public CollectionSettings()
    {
        DisplayBatchSize = 100;
    }
}
