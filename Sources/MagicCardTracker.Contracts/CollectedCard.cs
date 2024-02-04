using System;
using System.Text.Json.Serialization;

namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Represents a card in a collection.
    /// </summary>
    public class CollectedCard : Card, IEquatable<CollectedCard>
    {
        /// <summary>
        ///     Creates a new instance of a collected card.
        /// </summary>
        /// <param name="setCode"> The set code. </param>
        /// <param name="number"> The card number. </param>
        /// <param name="languageCode"> The language code. </param>
        [JsonConstructorAttribute]
        public CollectedCard(string setCode, string number, string languageCode) 
            : base(setCode, number, languageCode)
        {
        }

        /// <summary>
        ///     Creates a new instance of a collected card with a known count.
        /// </summary>
        /// <param name="card"> The card. </param>
        /// <param name="count"> Number of collected cards. </param>
        /// <param name="foilCount"> Number of collected foil cards. </param>
        /// <returns></returns>
        public CollectedCard(
            Card card, 
            int count,
            int foilCount) : base(card)
        {
            Count = count;
            FoilCount = foilCount;
        }

        /// <summary>
        ///     Gets or sets the count of this card in a collection.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Gets or sets the foil count of this card in a collection.
        /// </summary>
        public int FoilCount { get; set; }

        /// <summary>
        ///     Gets or sets total count of this card in a collection.
        /// </summary>
        public int TotalCount => Count + FoilCount;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                SetCode.ToLowerInvariant(), 
                Number.ToLowerInvariant(), 
                LanguageCode.ToLowerInvariant());
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            return other is CollectedCard card
                ? Equals(card)
                : false;
        }

        /// <inheritdoc/>
        public bool Equals(CollectedCard? other)
        {
            return other != null
                && SetCode.Equals(other.SetCode, StringComparison.OrdinalIgnoreCase)
                && Number.Equals(other.Number, StringComparison.OrdinalIgnoreCase)
                && LanguageCode.Equals(other.LanguageCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Calculates the value for a single card. 
        ///     In case you have both collected (Standard and Foil), 
        ///     the higher value is returned.
        /// </summary>
        /// <param name="currency"> The currency that should be used. </param>
        /// <returns> The value for this card. </returns>
        public decimal GetSingleCardValue(Currency currency)
        {
            var standardValue = Count > 0
                ? Prices.CalculateValue(currency, isFoilCard: false)
                : 0;
            
            var foilValue = FoilCount > 0
                ? Prices.CalculateValue(currency, isFoilCard: true)
                : 0;

            return Math.Round(Math.Max(standardValue, foilValue), 2);
        }

        /// <summary>
        ///     Calculates the collection value for this this card.
        /// </summary>
        /// <param name="currency"> The currency that should be used. </param>
        /// <returns> The total collection value for this card. </returns>
        public decimal GetCollectionValue(Currency currency)
        {
            return Prices.CalculateValue(currency, isFoilCard: false) * Count + 
                   Prices.CalculateValue(currency, isFoilCard: true) * FoilCount;
        }
    }
}
