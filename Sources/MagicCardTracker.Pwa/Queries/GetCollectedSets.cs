using System.Collections.Generic;
using MagicCardTracker.Pwa.Models;
using MediatR;

namespace MagicCardTracker.Pwa.Queries
{
    internal class GetCollectedSets : IRequest<IEnumerable<Set>>
    {
        public bool ForceRefresh { get; }

        public GetCollectedSets(bool forceRefresh = false)
        {
            ForceRefresh = forceRefresh;
        }
    }
}
