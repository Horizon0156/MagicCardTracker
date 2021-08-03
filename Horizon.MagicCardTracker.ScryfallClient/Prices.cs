using Newtonsoft.Json;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public class Prices
    {
        [JsonProperty("eur")]
        public decimal? Euros { get; set; }

        [JsonProperty("usd")]
        public decimal? Dollars { get; set; }

        [JsonProperty("eur_foil")]
        public decimal? EurosFoiled { get; set; }

        [JsonProperty("usd_foil")]
        public decimal? DollarsFoiled { get; set; }
    }
}
