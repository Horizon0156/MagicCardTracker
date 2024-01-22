using AutoMapper;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Models;

namespace MagicCardTracker.Pwa.Profiles;

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
            .ForCtorParam("setCode", cfg => cfg.MapFrom(c => c.Set))
            .ForCtorParam("number", cfg => cfg.MapFrom(c => c.Collector_number))
            .ForCtorParam("languageCode", cfg => cfg.MapFrom(c => c.Lang))
            .ForMember(c => c.CardType, cfg => cfg.MapFrom(c => FlattenCardType(c)))
            .ForMember(c => c.Colors, cfg => cfg.MapFrom(c => c.Color_identity))
            .ForMember(c => c.HasFoilVersion, cfg => cfg.MapFrom(c => c.Foil))
            .ForMember(c => c.ScryfallId, cfg => cfg.MapFrom(c => c.Id))
            .ForMember(c => c.ImageUrl, cfg => cfg.MapFrom(c => ExtractImageUrls(c, false)))
            .ForMember(c => c.FlipsideImageUrl, cfg => cfg.MapFrom(c => ExtractImageUrls(c, true)))
            .ForMember(c => c.ReleaseAt, cfg => cfg.MapFrom(c => c.Released_at))
            .ForMember(c => c.Rarity, cfg => cfg.MapFrom(c => c.Rarity))
            .ForMember(c => c.Legalities, cfg => cfg.MapFrom(c => FlattenLegality(c.Legalities)))
            .ForMember(c => c.Name, cfg => cfg.MapFrom(c => c.Name));

        CreateMap<ScryfallClient.Set, Set>()
            .ForCtorParam("code", cfg => cfg.MapFrom(s => s.Code))
            .ForCtorParam("name", cfg => cfg.MapFrom(s => s.Name))
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

    private CardType FlattenCardType(ScryfallClient.Card card)
    {
        if (card.Type_line.Contains("Creature", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Creature;
        }
        if (card.Type_line.Contains("Planeswalker", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Planeswalker;
        }
        if (card.Type_line.Contains("Enchantment", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Enchantment;
        }
        if (card.Type_line.Contains("Instant", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Instant;
        }
        if (card.Type_line.Contains("Land", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Land;
        }
        if (card.Type_line.Contains("Artifact", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Artifact;
        }
        if (card.Type_line.Contains("Sorcery", StringComparison.InvariantCultureIgnoreCase)) {
            return CardType.Sorcery;
        }

        return CardType.Other;
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

    private static string? ExtractImageUrls(ScryfallClient.Card card, bool useFlipsideImage)
    {
        var cardFaces = card.Card_faces?.ToArray();

        switch (card.Layout)
        {
            case ScryfallClient.CardLayout.Transform:
            case ScryfallClient.CardLayout.Split:
            case ScryfallClient.CardLayout.Flip:
            case ScryfallClient.CardLayout.Double_faced_token:
            case ScryfallClient.CardLayout.Reversible_card:
            case ScryfallClient.CardLayout.Modal_dfc:
                return useFlipsideImage 
                    ? cardFaces?.ElementAt(1)?.Image_uris.Normal?.ToString()
                    : cardFaces?.ElementAt(0)?.Image_uris.Normal?.ToString();
            default:
                return useFlipsideImage
                    ? null
                    : card.Image_uris.Normal.ToString();
        }
    }
}