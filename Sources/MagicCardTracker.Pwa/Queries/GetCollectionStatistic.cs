
using MagicCardTracker.Contracts;
using MagicCardTracker.Pwa.Models;
using MediatR;

namespace MagicCardTracker.Pwa.Queries;

internal class GetCollectionStatistic : IRequest<CollectionStatistic>
{
    public GetCollectionStatistic(Currency dominatingCurrency = Currency.Dollar)
    {
        DominatingCurrency = dominatingCurrency;
    }

    public Currency DominatingCurrency { get; }
}