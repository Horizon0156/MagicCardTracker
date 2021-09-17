
namespace MagicCardTracker.Contracts
{
    /// <summary>
    ///     Extensions for the <see href="Currency"/> enumeration.
    /// </summary>
    public static class CurrencyExtensions
    {
        /// <summary>
        ///     Prints the currency as a symbol. 
        /// </summary>
        /// <param name="currency"> This currency that has to be printed. </param>
        /// <returns> Currency symbol as a string. </returns>
        public static string ToCurrencySymbol(this Currency currency)
        {
            switch (currency)
            {
                case Currency.Dollar:
                    return "$";
                case Currency.Euro: 
                    return "€";
                default:
                    return string.Empty;
            }
        }
    }
}
