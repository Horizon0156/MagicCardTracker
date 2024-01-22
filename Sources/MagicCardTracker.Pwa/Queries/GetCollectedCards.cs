using MagicCardTracker.Contracts;
using MediatR;

namespace MagicCardTracker.Pwa.Queries;

internal class GetCollectedCards : IRequest<IEnumerable<CollectedCard>>
{
}
