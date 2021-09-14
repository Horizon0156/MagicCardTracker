using System.Linq;
using AutoMapper;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Models;

namespace MagicCardTracker.Pwa.Profiles
{
    internal class ScryfallApiProfile : Profile
    {
        private const decimal EURO_DOLLAR_EXCHANGE_RATE = 1.18M;

        public ScryfallApiProfile()
        {
            CreateMap<ScryfallClient.Prices, Contracts.PricingInformation>()
                .ForMember(p => p.StandardInEuros, cfg => cfg.MapFrom(p => p.Eur))
                .ForMember(p => p.StandardInDollars, cfg => cfg.MapFrom(p => p.Usd))
                .ForMember(p => p.FoiledInEuros, cfg => cfg.MapFrom(p => p.Eur_foil))
                .ForMember(p => p.FoiledInDollars, cfg => cfg.MapFrom(p => p.Usd_foil))
                .AfterMap(EnrichMissingPricingInformation);

            CreateMap<ScryfallClient.Card, Contracts.Card>()
                .ForMember(c => c.SetCode, cfg => cfg.MapFrom(c => c.Set))
                .ForMember(c => c.Number, cfg => cfg.MapFrom(c => c.Collector_number))
                .ForMember(c => c.LanguageCode, cfg => cfg.MapFrom(c => c.Lang))
                .ForMember(c => c.HasFoilVersion, cfg => cfg.MapFrom(c => c.Foil))
                .ForMember(c => c.ScryfallId, cfg => cfg.MapFrom(c => c.Id))
                .ForMember(c => c.ImageUrl, cfg => cfg.MapFrom(c => ExtractImageUrls(c, false)))
                .ForMember(c => c.FlipsideImageUrl, cfg => cfg.MapFrom(c => ExtractImageUrls(c, true)))
                .ForMember(c => c.ReleaseAt, cfg => cfg.MapFrom(c => c.Released_at))
                .ForMember(c => c.Rarity, cfg => cfg.MapFrom(c => c.Rarity))
                .ForMember(c => c.Legalities, cfg => cfg.MapFrom(c => FlattenLegality(c.Legalities)))
                .ForMember(c => c.Name, cfg => cfg.MapFrom(c => NormalizeName(c)));

            CreateMap<ScryfallClient.Set, Set>()
                .ForMember(s => s.Code, cfg => cfg.MapFrom(s => s.Code))
                .ForMember(s => s.Name, cfg => cfg.MapFrom(s => s.Name))
                .ForMember(s => s.ReleaseDate, cfg => cfg.MapFrom(s => s.Released_at.DateTime))
                .ForMember(s => s.SetIconUrl, cfg => cfg.MapFrom(s => s.Icon_svg_uri))
                .ForMember(s => s.NumberOfCards, cfg => cfg.MapFrom(s => s.Card_count))
                .ForMember(s => s.IsCoreOrExpansionSet, cfg => cfg.MapFrom(
                    s => s.Set_type == ScryfallClient.Set_type.Core 
                      || s.Set_type == ScryfallClient.Set_type.Expansion))
                .ForMember(s => s.NumberOfCollectedCards, cfg => cfg.Ignore());
        }

        private void EnrichMissingPricingInformation(
            ScryfallClient.Prices scryfallPrices,
            PricingInformation mappedPrices)
        {
            if (!mappedPrices.HasPricingInformation)
            {
                return;
            }

            if (!mappedPrices.StandardInEuros.HasValue && mappedPrices.StandardInDollars.HasValue)
            {
                mappedPrices.StandardInEuros = mappedPrices.StandardInDollars / EURO_DOLLAR_EXCHANGE_RATE;
            }
            else if (!mappedPrices.StandardInDollars.HasValue && mappedPrices.StandardInEuros.HasValue)
            {
                mappedPrices.StandardInDollars = mappedPrices.StandardInEuros * EURO_DOLLAR_EXCHANGE_RATE;
            }

            if (!mappedPrices.FoiledInEuros.HasValue && mappedPrices.FoiledInDollars.HasValue)
            {
                mappedPrices.FoiledInEuros = mappedPrices.FoiledInDollars / EURO_DOLLAR_EXCHANGE_RATE;
            }
            else if (!mappedPrices.FoiledInDollars.HasValue && mappedPrices.FoiledInEuros.HasValue)
            {
                mappedPrices.FoiledInDollars = mappedPrices.FoiledInEuros * EURO_DOLLAR_EXCHANGE_RATE;
            }
        }

        private Legality FlattenLegality(ScryfallClient.Legality scryfallLegality)
        {
            Legality legalities = 0;

            if (scryfallLegality.Standard == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Standard;
            }
            if (scryfallLegality.Commander == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Commander;
            }
            if (scryfallLegality.Modern == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Modern;
            }
            if (scryfallLegality.Brawl == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Brawl;
            }
            if (scryfallLegality.Historic == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Historic;
            }
            if (scryfallLegality.Pauper == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Pauper;
            }
            if (scryfallLegality.Penny == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Penny;
            }
            if (scryfallLegality.Pioneer == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Pioneer;
            }
            if (scryfallLegality.Vintage == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Vintage;
            }
            if (scryfallLegality.Legacy == ScryfallClient.Legal_status.Legal) {
                legalities |= Legality.Legacy;
            }


            return legalities;
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

            switch (card.Layout)
            {
                case ScryfallClient.CardLayout.Transform:
                case ScryfallClient.CardLayout.Split:
                case ScryfallClient.CardLayout.Flip:
                case ScryfallClient.CardLayout.Double_faced_token:
                case ScryfallClient.CardLayout.Double_sided:
                case ScryfallClient.CardLayout.Modal_dfc:
                    return $"{cardFaces[0].Printed_name ?? cardFaces[0].Name} " + 
                           $"// {cardFaces[1].Printed_name ?? cardFaces[1].Name}";
                default:
                    return card.Printed_name ?? card.Name;
            }
        }

        private static string ExtractImageUrls(ScryfallClient.Card card, bool useFlipsideImage)
        {
            var cardFaces = card.Card_faces?.ToArray();

            switch (card.Layout)
            {
                case ScryfallClient.CardLayout.Transform:
                case ScryfallClient.CardLayout.Split:
                case ScryfallClient.CardLayout.Flip:
                case ScryfallClient.CardLayout.Double_faced_token:
                case ScryfallClient.CardLayout.Double_sided:
                case ScryfallClient.CardLayout.Modal_dfc:
                    return useFlipsideImage 
                        ? cardFaces[1].Image_uris.Normal.ToString()
                        : cardFaces[0].Image_uris.Normal.ToString();
                default:
                    return useFlipsideImage
                        ? null
                        : card.Image_uris.Normal.ToString();
            }
        }
    }
}