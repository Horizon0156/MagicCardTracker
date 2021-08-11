
using System;

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
        public CollectedCard()
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
        public override bool Equals(object other)
        {
            return other is CollectedCard card
                ? Equals(card)
                : false;
        }

        /// <inheritdoc/>
        public bool Equals(CollectedCard other)
        {
            return other != null
                && SetCode.Equals(other.SetCode, StringComparison.OrdinalIgnoreCase)
                && Number.Equals(other.Number, StringComparison.OrdinalIgnoreCase)
                && LanguageCode.Equals(other.LanguageCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}
