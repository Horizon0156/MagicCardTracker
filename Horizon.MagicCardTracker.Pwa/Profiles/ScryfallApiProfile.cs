
using AutoMapper;

namespace Horizon.MagicCardTracker.Pwa.Profiles
{
    internal class ScryfallApiProfile : Profile
    {
        public ScryfallApiProfile()
        {
            CreateMap<ScryfallClient.Prices, Contracts.PricingInformation>()
                .ForMember(p => p.StandardInEuros, cfg => cfg.MapFrom(p => p.Euros))
                .ForMember(p => p.StandardInDollars, cfg => cfg.MapFrom(p => p.Dollars))
                .ForMember(p => p.FoiledInEuros, cfg => cfg.MapFrom(p => p.EurosFoiled))
                .ForMember(p => p.FoiledInDollars, cfg => cfg.MapFrom(p => p.DollarsFoiled));

            CreateMap<ScryfallClient.Card, Contracts.Card>()
                .ForMember(c => c.HasFoilVersion, cfg => cfg.MapFrom(c => c.HasFoil))
                .ForMember(c => c.ScryfallId, cfg => cfg.MapFrom(c => c.Id))
                .ForMember(c => c.ImageUrl, cfg => cfg.MapFrom(c => NormalizeImageUrl(c, false)))
                .ForMember(c => c.FlipsideImageUrl, cfg => cfg.MapFrom(c => NormalizeImageUrl(c, true)))
                .ForMember(c => c.Name, cfg => cfg.MapFrom(c => NormalizeName(c)));
        }

        private static string NormalizeName(ScryfallClient.Card card)
        {
            return card.Layout == ScryfallClient.KnownLayouts.Transform    
                ? $"{card.Faces[0].Name ?? card.Faces[0].OriginalName} " + 
                  $"// {card.Faces[1].Name ?? card.Faces[1].OriginalName}"
                : card.Name ?? card.OriginalName;
        }

        private static string NormalizeImageUrl(ScryfallClient.Card card, bool useBackside)
        {
            return card.Layout == ScryfallClient.KnownLayouts.Transform
                ? useBackside 
                    ? card.Faces[1].Images.Normal 
                    : card.Faces[0].Images.Normal
                : card.Images.Normal;
        }
    }
}