using System.Collections.Generic;
using MagicCardTracker.Pwa.Models;
using MediatR;

namespace MagicCardTracker.Pwa.Queries
{
    internal class GetSetsWithCollectionInfo : IRequest<IEnumerable<Set>>
    {
    }
}
