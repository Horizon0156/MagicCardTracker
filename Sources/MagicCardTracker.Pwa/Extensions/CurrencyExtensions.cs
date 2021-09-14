using MagicCardTracker.Contracts;

namespace MagicCardTracker.Pwa.Extensions
{
    internal static class CurrencyExtensions
    {
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
