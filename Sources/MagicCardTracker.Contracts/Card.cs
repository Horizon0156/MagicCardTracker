using System;

namespace MagicCardTracker.Contracts
{
    public class Card
    {
        /// <summary>
        ///     Creates a new instance of a card.
        /// </summary>
        public Card()
        {
            
        }
        
        /// <summary>
        ///     Creates a copy of the given card.
        /// </summary>
        /// <param name="card"> The card that should be copied into a new instance. </param>
        public Card(Card card)
        {
            SetCode = card.SetCode;
            Number = card.Number;
            LanguageCode = card.LanguageCode;
            ScryfallId = card.ScryfallId;
            
            UpdateMutualProperties(card);
        }

        public static string OriginalLanguageCode => "en";

        public bool HasFoilVersion { get; set; }

        public PricingInformation Prices { get; set; }

        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public string ImageUrl { get; set; }

        public string FlipsideImageUrl { get; set; }

        public string SetCode { get; set; }

        public string Number { get; set; }

        public string ManaCosts { get; set; }

        public Legality Legalities { get; set; }

        public string[] Colors { get; set; }

        public string ScryfallId { get; set; }

        public CardRarity Rarity { get; set; }

        public DateTimeOffset ReleaseAt { get; set; }

        public void UpdatePrices(PricingInformation prices)
        {
            Prices = prices != null && prices.HasPricingInformation
                ? prices
                : Prices;
        }

        public void UpdateMutualProperties(Card card)
        {
            UpdatePrices(card.Prices);

            Name = card.Name;
            HasFoilVersion = card.HasFoilVersion;
            ImageUrl = card.ImageUrl;
            FlipsideImageUrl = card.FlipsideImageUrl;
            ManaCosts = card.ManaCosts;
            Colors = card.Colors;
            ReleaseAt = card.ReleaseAt;
            Rarity = card.Rarity;
            Legalities = card.Legalities;
        }
    }
}
