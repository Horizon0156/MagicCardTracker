using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Horizon.MagicCardTracker.Contracts;
using Horizon.MagicCardTracker.Pwa.Commands;
using Horizon.MagicCardTracker.Pwa.Queries;
using Horizon.MagicCardTracker.ScryfallClient;
using Horizon.MagicCardTracker.SrcyfallClient.Extensions;
using Horizon.MagicCardTracker.Storage;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Handlers
{
    internal class CardCollectionHandler : 
        AsyncRequestHandler<AddCard>, 
        IRequestHandler<AddCardByNumber, CollectedCard>,
        IRequestHandler<GetCollectedCards, IEnumerable<CollectedCard>>,
        IRequestHandler<GetCollectableCard, CollectedCard>
    {
        private readonly ICardLibrary _cardLibrary;
        private readonly IScryfallClient _scryfallClient;

        public CardCollectionHandler(
            ICardLibrary cardLibrary,
            IScryfallClient scryfallClient)
        {
            _cardLibrary = cardLibrary;
            _scryfallClient = scryfallClient;
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

            var desiredCard = await _scryfallClient.GetCardByNumberAsync(
                request.SetCode, 
                request.CardNumber, 
                request.LanguageCode, 
                cancellationToken);
            var card = desiredCard.ToContract();
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

            var originalCard = await _scryfallClient.GetCardByNumberAsync(card.SetCode, card.Number, "en", cancellationToken);

            if (originalCard != null)
            {
                card.Prices = originalCard.ToContract().Prices;
            }
        }
    }
}
