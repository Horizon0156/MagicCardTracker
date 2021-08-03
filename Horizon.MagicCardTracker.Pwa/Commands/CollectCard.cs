using Horizon.MagicCardTracker.Contracts;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Commands
{
    public class AddCard : IRequest
    {
        public AddCard(CollectedCard card)
        {
            Card = card;
        }

        public CollectedCard Card { get; }
    }
}
