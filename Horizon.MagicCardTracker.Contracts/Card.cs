﻿namespace Horizon.MagicCardTracker.Contracts
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
            Name = card.Name;
            LanguageCode = card.LanguageCode;
            HasFoilVersion = card.HasFoilVersion;
            ImageUrl = card.ImageUrl;
            FlipsideImageUrl = card.FlipsideImageUrl;
            Prices = card.Prices;
            ManaCosts = card.ManaCosts;
            Colors = card.Colors;
            ScryfallId = card.ScryfallId;
        }

        public bool HasFoilVersion { get; set; }

        public PricingInformation Prices { get; set; }

        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public string ImageUrl { get; set; }

        public string FlipsideImageUrl { get; set; }

        public string SetCode { get; set; }

        public string Number { get; set; }

        public string ManaCosts { get; set; }

        public string[] Colors { get; set; }

        public string ScryfallId { get; set; }
    }
}
