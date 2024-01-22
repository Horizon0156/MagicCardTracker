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
        /// <param name="currency">
        ///     The currency that should be printed.
        /// </param>
        /// <param name="isFoilCard"> 
        ///     Flag indicating whether foil value should be printed.
        /// </param>
        /// <returns> Pricing information as string. </returns>
        public string ToString(Currency currency, bool isFoilCard)
        {
            return $"{currency.ToCurrencySymbol()}" +
                   $"{CalculateValue(currency, isFoilCard).ToString("F2")}";
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
    }
}
