namespace Horizon.MagicCardTracker.SrcyfallClient.Extensions
{
    public static class CardExtensions
    {
        public static Contracts.Card ToContract(this ScryfallClient.Card card)
        {
            return new Contracts.Card
            {
                HasFoilVersion = card.HasFoil, 
                Name = card.NormalizeName(),
                LanguageCode = card.LanguageCode,
                ImageUrl = card.NormalizeImageUrl(),
                FlipsideImageUrl = card.NormalizeFlipsideImageUrl(),
                SetCode = card.SetCode,
                Number = card.Number,
                ManaCosts = card.ManaCosts,
                Colors = card.ColorCodes,
                ScryfallId = card.Id,
                Prices = new Contracts.PricingInformation
                {
                    StandardInEuros = card.Prices?.Euros,
                    StandardInDollars = card.Prices?.Dollars,
                    FoiledInEuros = card.Prices?.EurosFoiled,
                    FoiledInDollars = card.Prices?.DollarsFoiled,
                }
            };
        }

        public static string NormalizeName(this ScryfallClient.Card card)
        {
            return card.Layout == ScryfallClient.KnownLayouts.Transform    
                ? $"{card.Faces[0].Name ?? card.Faces[0].OriginalName} " + 
                  $"// {card.Faces[1].Name ?? card.Faces[1].OriginalName}"
                : card.Name ?? card.OriginalName;
        }

        public static string NormalizeImageUrl(this ScryfallClient.Card card)
        {
            return card.Layout == ScryfallClient.KnownLayouts.Transform
                ? card.Faces[0].Images.Normal
                : card.Images.Normal;
        }

        public static string NormalizeFlipsideImageUrl(this ScryfallClient.Card card)
        {
            return card.Layout == ScryfallClient.KnownLayouts.Transform
                ? card.Faces[1].Images.Normal
                : null;
        }
    }
}
