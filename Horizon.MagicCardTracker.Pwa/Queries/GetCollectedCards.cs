using System.Collections.Generic;
using Horizon.MagicCardTracker.Contracts;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Queries
{
    internal class GetCollectedCards : IRequest<IEnumerable<CollectedCard>>
    {
    }
}
