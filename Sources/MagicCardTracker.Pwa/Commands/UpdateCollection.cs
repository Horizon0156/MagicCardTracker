using MagicCardTracker.Contracts;
using MediatR;

namespace MagicCardTracker.Pwa.Commands;

internal class UpdateCollection : IRequest
{
    public UpdateMode Mode { get; }

    public UpdateCollection(UpdateMode mode = UpdateMode.Prices)
    {
        Mode = mode;
    }
}
