
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Horizon.MagicCardTracker.Pwa.Models;
using Horizon.MagicCardTracker.Pwa.Queries;
using Horizon.MagicCardTracker.ScryfallClient;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Handlers
{
    internal class CardSearchHandler : IRequestHandler<SearchCards, CardSearchResult>
    {
        private readonly IScryfallClient _scryfallClient;
        private readonly IMapper _mapper;

        public CardSearchHandler(
            IScryfallClient scryfallClient,
            IMapper mapper)
        {
            _scryfallClient = scryfallClient;
            _mapper = mapper;
        }
        public async Task<CardSearchResult> Handle(SearchCards request, CancellationToken cancellationToken)
        {
            var query = request.Query.Replace(" ", "+");
            var searchResult = await _scryfallClient.SearchCardsAsync(query, true, cancellationToken);

            return new CardSearchResult
            {
                HasMoreResults = searchResult.HasMoreCards,
                NumberOfMatchedCards = searchResult.TotalCards,
                Cards = _mapper.Map<IEnumerable<Contracts.Card>>(searchResult.Cards)
            };
        }
    }
}