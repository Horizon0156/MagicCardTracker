using System.Collections.Generic;
using Horizon.MagicCardTracker.Pwa.Models;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Queries
{
    internal class GetCollectedSets : IRequest<IEnumerable<Set>>
    {
    }
}
