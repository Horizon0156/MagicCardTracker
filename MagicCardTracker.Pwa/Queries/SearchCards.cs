
using MagicCardTracker.Pwa.Models;
using MediatR;

namespace MagicCardTracker.Pwa.Queries
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