using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Commands;
using MagicCardTracker.Pwa.Queries;
using MagicCardTracker.ScryfallClient;
using MagicCardTracker.Storage;
using AutoMapper;
using MediatR;

namespace MagicCardTracker.Pwa.Handlers
{
    internal class CardCollectionHandler : 
        AsyncRequestHandler<AddCard>, 
        IRequestHandler<AddCardByNumber, CollectedCard>,
        IRequestHandler<GetCollectedCards, IEnumerable<CollectedCard>>,
        IRequestHandler<GetCollectableCard, CollectedCard>
    {
        private readonly ICardLibrary _cardLibrary;
        private readonly IScryfallClientFactory _scryfallClientFactory;
        private readonly IMapper _mapper;

        public CardCollectionHandler(
            ICardLibrary cardLibrary,
            IScryfallClientFactory scryfallClientFactory,
            IMapper mapper)
        {
            _cardLibrary = cardLibrary;
            _scryfallClientFactory = scryfallClientFactory;
            _mapper = mapper;
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
            var collectedCard = await _cardLibrary.SearchInCollectionByIdAsync(
                request.SetCode,
                request.CardNumber,
                request.LanguageCode,
                cancellationToken
            );

            if (collectedCard != null)
            {
                collectedCard.Count++;
                await _cardLibrary.AddCardAsync(collectedCard, cancellationToken);

                return collectedCard;
            }

            var desiredCard = await _scryfallClientFactory
                                .Cards
                                .GetByCodeByNumberByLangAsync(
                                    request.SetCode, 
                                    request.CardNumber, 
                                    request.LanguageCode, 
                                    cancellationToken);
            var card = _mapper.Map<Contracts.Card>(desiredCard);
            await EnrichPricingInformationIfApplicableAsync(card, cancellationToken);

            var collectableCard = new CollectedCard(card, 1, 0);
            await _cardLibrary.AddCardAsync(collectableCard, cancellationToken);
            return collectableCard;
        }

        protected override Task Handle(AddCard request, CancellationToken cancellationToken)
        {
            return _cardLibrary.AddCardAsync(request.Card, cancellationToken);
        }

        private async Task EnrichPricingInformationIfApplicableAsync(Contracts.Card card, CancellationToken cancellationToken)
        {
            // With Scryfall foreign cards usually do not have pricing information,
            // so lets try to pick those from the original card (en)
            if (card.Prices.HasPricingInformation || card.LanguageCode == "en")
            {
                return;
            }

            var originalCard = await _scryfallClientFactory
                                        .Cards
                                        .GetByCodeByNumberAsync(card.SetCode, card.Number, cancellationToken);

            if (originalCard != null)
            {
                card.Prices = _mapper.Map<PricingInformation>(originalCard.Prices);
            }
        }
    }
}