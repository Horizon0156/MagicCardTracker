using Newtonsoft.Json;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public class CardFace 
    {
        [JsonProperty("name")]
        public string OriginalName { get; set; }

        [JsonProperty("printed_name")]
        public string Name { get; set; }

        [JsonProperty("image_uris")]
        public ImageUrls Images { get; set; }

        [JsonProperty("mana_cost")]
        public string ManaCosts { get; set; }
    }
}
