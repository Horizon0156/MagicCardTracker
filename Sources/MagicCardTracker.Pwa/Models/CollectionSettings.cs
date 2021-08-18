namespace MagicCardTracker.Pwa.Models
{
    internal class CollectionSettings
    {
        public static string Key => "Collection";
        
        public int DisplayBatchSize { get; set; }

        public CollectionSettings()
        {
            DisplayBatchSize = 100;
        }
    }
}
