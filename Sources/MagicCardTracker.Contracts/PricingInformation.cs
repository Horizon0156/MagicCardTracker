using System.Text;

namespace MagicCardTracker.Contracts
{
    public class PricingInformation
    {
        public decimal? StandardInEuros { get; set; }

        public decimal? StandardInDollars { get; set; }

        public decimal? FoiledInEuros { get; set; }

        public decimal? FoiledInDollars { get; set; }

        public bool HasPricingInformation => 
            StandardInEuros.HasValue ||
            StandardInDollars.HasValue ||
            FoiledInEuros.HasValue ||
            FoiledInDollars.HasValue;

        public string ToString(bool printFoilValue)
        {
            
            return printFoilValue
                ? FoilValueToString()
                : StandardValueToString();
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
