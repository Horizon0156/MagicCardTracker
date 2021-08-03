
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public class CardList
    {
        [JsonProperty("total_cards")]
        public int TotalCards { get; set; }

        [JsonProperty("has_more")]
        public bool HasMoreCards { get; set; }

        [JsonProperty("next_page")]
        public string NextPageUrl { get; set; }

        [JsonProperty("data")]
        public IEnumerable<Card> Cards { get; set; }
    }
}