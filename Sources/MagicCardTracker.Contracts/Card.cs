using System;

namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Represents a Magic card
    /// </summary>
    public class Card
    {
        /// <summary>
        ///     Creates a new instance of a card.
        /// </summary>
        /// <param name="setCode"> The set code. </param>
        /// <param name="number"> The card number. </param>
        /// <param name="languageCode"> The language code. </param>
        public Card(string setCode, string number, string languageCode)
        {   
            SetCode = setCode;
            Number = number;
            LanguageCode = languageCode;

            Prices = new PricingInformation();
            Colors = new string[0];
        }
        
        /// <summary>
        ///     Creates a copy of the given card.
        /// </summary>
        /// <param name="card"> The card that should be copied into a new instance. </param>
        public Card(Card card) : this(card.SetCode, card.Number, card.LanguageCode)
        {
            ScryfallId = card.ScryfallId;   
            UpdateMutualProperties(card);
        }

        /// <summary>
        ///     Language code for cards printed in original language.
        /// </summary>
        public static string OriginalLanguageCode => "en";

        /// <summary>
        ///     Gets or sets a flag indicating whether this card has a foil version.
        /// </summary>
        public bool HasFoilVersion { get; set; }

        /// <summary>
        ///     Gets or sets the market prices of this card.
        /// </summary>
        public PricingInformation Prices { get; set; }

        /// <summary>
        ///     Gets or sets the printed name of this card.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets or sets the language code of this card.
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        ///     Gets or sets the image url of this card.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        ///     Gets or sets the image url of the back of this card.
        /// </summary>
        public string? FlipsideImageUrl { get; set; }

        /// <summary>
        ///     Gets or sets the set code.
        /// </summary>
        public string SetCode { get; }

        /// <summary>
        ///     Gets or sets the collector number.
        /// </summary>
        public string Number { get; }

        /// <summary>
        ///     Gets or sets the mana costs
        /// </summary>
        public string? ManaCosts { get; set; }

        /// <summary>
        ///     Gets or sets the legalities of this card.
        /// </summary>
        public Legality Legalities { get; set; }

        /// <summary>
        ///     Gets or sets the colors (mana) of this card.
        /// </summary>
        public string[] Colors { get; set; }

        /// <summary>
        ///     Gets or sets the Scryfall identifier.
        /// </summary>
        public string? ScryfallId { get; set; }

        /// <summary>
        ///     Gets or sets the rarity.
        /// </summary>
        public CardRarity Rarity { get; set; }

        /// <summary>
        ///     Gets or sets the rarity.
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        ///     Gets or sets the release date of this card.
        /// </summary>
        public DateTimeOffset ReleaseAt { get; set; }

        /// <summary>
        ///     Updates the pricing information of this card.
        /// </summary>
        /// <param name="updatedPrices"> Recent market prices. </param>
        public void UpdatePrices(PricingInformation updatedPrices)
        {
            Prices = updatedPrices.HasPricingInformation
                ? updatedPrices
                : new PricingInformation();
        }

        /// <summary>
        ///     Updates all mutable fields of this card.
        /// </summary>
        /// <param name="card"> A card holding the recent values to update. </param>
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
            CardType = card.CardType;
        }
    }
}
