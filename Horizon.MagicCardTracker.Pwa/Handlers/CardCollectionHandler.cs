using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Horizon.MagicCardTracker.Contracts;
using Horizon.MagicCardTracker.Pwa.Commands;
using Horizon.MagicCardTracker.Pwa.Queries;
using Horizon.MargicCardTracker.Storage;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Handlers
{
    internal class CardCollectionHandler : 
        AsyncRequestHandler<AddCard>, 
        IRequestHandler<GetCollectedCards, IEnumerable<CollectedCard>>,
        IRequestHandler<GetCollectableCard, CollectedCard>
    {
        private readonly ICardLibrary _cardLibrary;

        public CardCollectionHandler(
            ICardLibrary cardLibrary)
        {
            _cardLibrary = cardLibrary;
        }

        public Task<IEnumerable<CollectedCard>> Handle(GetCollectedCards request, CancellationToken cancellationToken)
        {
            return _cardLibrary.GetCollectedCardsAsync(cancellationToken);
        }

        public async Task<CollectedCard> Handle(GetCollectableCard request, CancellationToken cancellationToken)
        {
            var collectableCard = await _cardLibrary.SearchInCollectionAsync(request.Card, cancellationToken);

            // With Scryfall foreign cards usually do not have pricing information,
            // so lets try to pick those from the original card (en)
            if (!collectableCard.Prices.HasPricingInformation && collectableCard.LanguageCode != "en")
            {
                collectableCard.Prices.StandardInEuros = (decimal) 1.23;
            }
            return collectableCard;
        }

        protected override Task Handle(AddCard request, CancellationToken cancellationToken)
        {
            return _cardLibrary.AddCardAsync(request.Card, cancellationToken);
        }
    }
}
