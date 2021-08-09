
using Horizon.MagicCardTracker.Pwa.Models;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Queries
{
    public class SearchCards : IRequest<CardSearchResult>
    {
        public SearchCards(string query)
        {
            Query = query;
        }
        public string Query { get; }
    }
}