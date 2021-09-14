#nullable enable

using System.Text;

namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Represents market prices of a card.
    /// </summary>
    public class PricingInformation
    {
        /// <summary>
        ///     Gets or sets the value in €
        /// </summary>
        public decimal? StandardInEuros { get; set; }

        /// <summary>
        ///     Gets or sets the value in $
        /// </summary>
        public decimal? StandardInDollars { get; set; }

        /// <summary>
        ///     Gets or sets the value in € for foils
        /// </summary>
        public decimal? FoiledInEuros { get; set; }

        /// <summary>
        ///     Gets or sets the value in $ for foils
        /// </summary>
        public decimal? FoiledInDollars { get; set; }

        /// <summary>
        ///     Gets a flag whether this instance is having pricing information.
        /// </summary>
        public bool HasPricingInformation => 
            StandardInEuros.HasValue ||
            StandardInDollars.HasValue ||
            FoiledInEuros.HasValue ||
            FoiledInDollars.HasValue;

        /// <summary>
        ///     Prints a human readable string.
        /// </summary>
        /// <param name="isFoilCard"> 
        ///     Flag indicating whether foil value should be printed.
        /// </param>
        /// <returns> Pricing information as string. </returns>
        public string ToString(bool isFoilCard)
        {
            
            return isFoilCard
                ? FoilValueToString()
                : StandardValueToString();
        }

        /// <summary>
        ///     Calculate value for this pricing information.
        /// </summary>
        /// <param name="currency"> The target currency. </param>
        /// <param name="isFoilCard"> Flag indicating whether foil value shall be used. </param>
        /// <returns> Value </returns>
        public decimal CalculateValue(Currency currency, bool isFoilCard)
        {
            var value = isFoilCard
                ? currency == Currency.Dollar ? FoiledInDollars : FoiledInEuros
                : currency == Currency.Dollar ? StandardInDollars : StandardInEuros;
            
            return value ?? 0;
        }

        private string StandardValueToString()
        {
            var stringBuilder = new StringBuilder();
            
            if (StandardInEuros.HasValue) 
            {
                stringBuilder.Append($"€{StandardInEuros.Value.ToString("F2")} ");
            }

            if (StandardInDollars.HasValue) 
            {
                stringBuilder.Append($"${StandardInDollars.Value.ToString("F2")} ");
            }

            return stringBuilder.ToString();
        }

        private string FoilValueToString()
        {
            var stringBuilder = new StringBuilder();
            
            if (FoiledInEuros.HasValue) 
            {
                stringBuilder.Append($"€{FoiledInEuros.Value.ToString("F2")} ");
            }

            if (FoiledInDollars.HasValue) 
            {
                stringBuilder.Append($"${FoiledInDollars.Value.ToString("F2")} ");
            }

            return stringBuilder.ToString();
        }
    }
}
