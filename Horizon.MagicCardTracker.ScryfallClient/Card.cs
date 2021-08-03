using Newtonsoft.Json;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public class Card
    {
        [JsonProperty("card_faces")]
        public Card[] Faces { get; set; }

        [JsonProperty("foil")]
        public bool HasFoil { get; set; }

        [JsonProperty("lang")]
        public string LanguageCode { get; set; }

        [JsonProperty("image_uris")]
        public ImageUrls Images { get; set; }

        [JsonProperty("set")]
        public string SetCode { get; set; }  

        [JsonProperty("collector_number")]
        public string Number { get; set; }

        [JsonProperty("mana_cost")]
        public string ManaCosts { get; set; }

        [JsonProperty("color_identity")]
        public string[] ColorCodes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string OriginalName { get; set; }

        [JsonProperty("printed_name")]
        public string Name { get; set; }

        [JsonProperty("prices")]
        public Prices Prices { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }
    }
}
