using Horizon.MagicCardTracker.Contracts;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Queries
{
    internal class GetCollectableCard : IRequest<CollectedCard>
    {
        public GetCollectableCard(Card card)
        {
            Card = card;
        }

        public Card Card { get; set; }
    }
}
