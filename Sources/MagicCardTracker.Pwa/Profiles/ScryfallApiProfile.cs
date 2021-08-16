using System;
using System.Linq;
using AutoMapper;
using MagicCardTracker.Pwa.Models;

namespace MagicCardTracker.Pwa.Profiles
{
    internal class ScryfallApiProfile : Profile
    {
        public ScryfallApiProfile()
        {
            CreateMap<ScryfallClient.Prices, Contracts.PricingInformation>()
                .ForMember(p => p.StandardInEuros, cfg => cfg.MapFrom(p => p.Eur))
                .ForMember(p => p.StandardInDollars, cfg => cfg.MapFrom(p => p.Usd))
                .ForMember(p => p.FoiledInEuros, cfg => cfg.MapFrom(p => p.Eur_foil))
                .ForMember(p => p.FoiledInDollars, cfg => cfg.MapFrom(p => p.Usd_foil));

            CreateMap<ScryfallClient.Card, Contracts.Card>()
                .ForMember(c => c.SetCode, cfg => cfg.MapFrom(c => c.Set))
                .ForMember(c => c.Number, cfg => cfg.MapFrom(c => c.Collector_number))
                .ForMember(c => c.LanguageCode, cfg => cfg.MapFrom(c => c.Lang))
                .ForMember(c => c.HasFoilVersion, cfg => cfg.MapFrom(c => c.Foil))
                .ForMember(c => c.ScryfallId, cfg => cfg.MapFrom(c => c.Id))
                .ForMember(c => c.ImageUrl, cfg => cfg.MapFrom(c => NormalizeImageUrl(c, false)))
                .ForMember(c => c.FlipsideImageUrl, cfg => cfg.MapFrom(c => NormalizeImageUrl(c, true)))
                .ForMember(c => c.Name, cfg => cfg.MapFrom(c => NormalizeName(c)));

            CreateMap<ScryfallClient.Set, Set>()
                .ForMember(s => s.Code, cfg => cfg.MapFrom(s => s.Code))
                .ForMember(s => s.Name, cfg => cfg.MapFrom(s => s.Name))
                .ForMember(s => s.ReleaseDate, cfg => cfg.MapFrom(s => s.Released_at.DateTime))
                .ForMember(s => s.SetIconUrl, cfg => cfg.MapFrom(s => s.Icon_svg_uri))
                .ForMember(s => s.NumberOfCards, cfg => cfg.MapFrom(s => s.Card_count))
                .ForMember(s => s.NumberOfCollectedCards, cfg => cfg.Ignore());
        }

        private decimal? ParsePrice(string price)
        {
            
            var result = decimal.TryParse(price, out var parsedPrice)
                ? parsedPrice
                : (decimal?) null;
            return result;
        }

        private static string NormalizeName(ScryfallClient.Card card)
        {
            var cardFaces = card.Card_faces?.ToArray();
            return card.Layout == ScryfallClient.CardLayout.Transform    
                ? $"{cardFaces[0].Printed_name ?? cardFaces[0].Name} " + 
                  $"// {cardFaces[1].Printed_name ?? cardFaces[1].Name}"
                : card.Printed_name ?? card.Name;
        }

        private static string NormalizeImageUrl(ScryfallClient.Card card, bool useBackside)
        {
            var cardFaces = card.Card_faces?.ToArray();
            return card.Layout == ScryfallClient.CardLayout.Transform  
                ? useBackside 
                    ? cardFaces[1].Image_uris.Normal.ToString()
                    : cardFaces[0].Image_uris.Normal.ToString()
                : card.Image_uris.Normal.ToString();
        }
    }
}