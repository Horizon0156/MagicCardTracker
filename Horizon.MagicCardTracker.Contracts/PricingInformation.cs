using System;

namespace Horizon.MagicCardTracker.Contracts
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
    }
}
