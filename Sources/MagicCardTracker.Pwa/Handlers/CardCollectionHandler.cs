using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Commands;
using MagicCardTracker.Pwa.Queries;
using MagicCardTracker.Pwa.Notifications;
using MagicCardTracker.ScryfallClient;
using MagicCardTracker.Storage;
using AutoMapper;
using MediatR;
using MagicCardTracker.Pwa.Exceptions;

namespace MagicCardTracker.Pwa.Handlers;

internal class CardCollectionHandler : 
    IRequestHandler<AddCard>, 
    IRequestHandler<AddCardByNumber, CollectedCard>,
    IRequestHandler<GetCollectedCards, IEnumerable<CollectedCard>>,
    IRequestHandler<GetCollectableCard, CollectedCard>,
    IRequestHandler<UpdateCollection>
{
    private readonly ICardLibrary _cardLibrary;
    private readonly IScryfallClientFactory _scryfallClientFactory;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public CardCollectionHandler(
        ICardLibrary cardLibrary,
        IScryfallClientFactory scryfallClientFactory,
        IMapper mapper,
        INotificationService notificationService)
    {
        _cardLibrary = cardLibrary;
        _scryfallClientFactory = scryfallClientFactory;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public Task<IEnumerable<CollectedCard>> Handle(GetCollectedCards request, CancellationToken cancellationToken)
    {
        return _cardLibrary.GetCollectedCardsAsync(cancellationToken);
    }

    public async Task<CollectedCard> Handle(GetCollectableCard request, CancellationToken cancellationToken)
    {
        var collectableCard = await _cardLibrary.SearchInCollectionAsync(request.Card, cancellationToken);
        await EnrichPricingInformationIfApplicableAsync(collectableCard, cancellationToken);

        return collectableCard;
    }

    public async Task<CollectedCard> Handle(AddCardByNumber request, CancellationToken cancellationToken)
    {
        var setCode = request.SetCode.ToLower().Trim();
        var cardNumber = request.CardNumber.TrimStart('0').Trim();
        var languageCode = request.LanguageCode.ToLower().Trim();

        var collectedCard = await _cardLibrary.SearchInCollectionByIdAsync(
            setCode,
            cardNumber,
            languageCode,
            cancellationToken
        );

        if (collectedCard != null)
        {
            if (request.AddAsFoil) 
            {
                collectedCard.FoilCount++;
            }
            else
            {
                collectedCard.Count++;
            }
            
            await _cardLibrary.AddCardAsync(collectedCard, cancellationToken);

            return collectedCard;
        }

        try
        {
            var desiredCard = await _scryfallClientFactory
                            .Cards
                            .GetByCodeByNumberByLangAsync(
                                setCode, 
                                cardNumber, 
                                languageCode, 
                                cancellationToken);
            var card = _mapper.Map<Contracts.Card>(desiredCard);
            await EnrichPricingInformationIfApplicableAsync(card, cancellationToken);

            var collectableCard = request.AddAsFoil
                ? new CollectedCard(card, 0, 1)
                : new CollectedCard(card, 1, 0);
            await _cardLibrary.AddCardAsync(collectableCard, cancellationToken);
            return collectableCard;
        }
        catch (ApiException e)
        {
            throw new CardNotFoundException($"{setCode}:{cardNumber} could not be found", e);
        }
    }

    public async Task Handle(UpdateCollection request, CancellationToken cancellationToken)
    {
        var library = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);

        switch (request.Mode)
        {
            case UpdateMode.Prices:
                await UpdateCollection(library, UpdateMode.Prices, cancellationToken);
                _notificationService.SendNotification( 
                    new Notification($"Updated prices for {library.Count()} cards"));
                break;
            case UpdateMode.AllMutableProperties:
                await UpdateCollection(
                    library,
                    UpdateMode.AllMutableProperties,
                    cancellationToken);
                _notificationService.SendNotification( 
                    new Notification($"Updated data for {library.Count()} cards"));

                // As pricing information is valid on EN cards,
                // we have to take a second round for foreign cards.
                var foreignCards = library.Where(
                                c => c.LanguageCode != Contracts.Card.OriginalLanguageCode);
                await UpdateCollection(
                    foreignCards,
                    UpdateMode.Prices,
                    cancellationToken
                );
                _notificationService.SendNotification( 
                    new Notification($"Updated prices for {library.Count()} cards"));
                break;
            default:
                break;
        }
    }

    public async Task Handle(AddCard request, CancellationToken cancellationToken)
    {
        await _cardLibrary.AddCardAsync(request.Card, cancellationToken);
    }

    private async Task EnrichPricingInformationIfApplicableAsync(
        Contracts.Card card,
        CancellationToken cancellationToken)
    {
        // With Scryfall foreign cards usually do not have pricing information,
        // so lets try to pick those from the original card (en)
        if (card.Prices.HasPricingInformation 
         || card.LanguageCode == Contracts.Card.OriginalLanguageCode)
        {
            return;
        }

        ScryfallClient.Card? originalCard;
        try
        {
            originalCard = await _scryfallClientFactory
                                    .Cards
                                    .GetByCodeByNumberAsync(card.SetCode, card.Number, cancellationToken);
        }
        catch (ApiException)
        {
            originalCard = null;
        }

        if (originalCard != null)
        {
            card.Prices = _mapper.Map<PricingInformation>(originalCard.Prices);
        }
    }

    private async Task UpdateCollection(
        IEnumerable<CollectedCard> cardsToUpdate,
        UpdateMode updateMode,
        CancellationToken cancellationToken)
    {
        // Scryfall supports a maximum of 75 card references per collection request
        // Therefore, we gracefully divide the card collection to chunks of 70 cards.
        var chunks = cardsToUpdate.Select((item, index) => new { index, item })
                            .GroupBy(x => x.index / 70)
                            .Select(g => g.Select(x => x.item));

        var updatedCards = new List<Contracts.Card>();
        foreach (var chunk in chunks)
        {
            // In case we just update prices, we are going to take the English version of the 
            // card to get latest price informations as foreign cards don't include those.
            var ids = chunk.Select(c => updateMode == UpdateMode.Prices
                        ? new Card_identifier { Set = c.SetCode, Collector_number = c.Number }
                        : new Card_identifier { Id = c.ScryfallId });

            var cards = await _scryfallClientFactory.Cards
                                                    .CollectionAsync(
                                                        new Card_collection_request 
                                                        { 
                                                            Identifiers = ids.ToArray()
                                                        },
                                                        cancellationToken);
            updatedCards.AddRange(cards.Data.Select(c => _mapper.Map<Contracts.Card>(c)));
        }

        await _cardLibrary.UpdatedCollectionAsync(updatedCards, updateMode, cancellationToken);
    }
}
