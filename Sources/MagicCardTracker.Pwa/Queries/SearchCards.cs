
using MagicCardTracker.Pwa.Models;
using MediatR;

namespace MagicCardTracker.Pwa.Queries
{
    public class SearchCards : IRequest<CardSearchResult>
    {
        public SearchCards(string query, int? page = null)
        {
            Query = query;
            Page = page;
        }

        public string Query { get; }

        public int? Page { get; set; }
    }
}