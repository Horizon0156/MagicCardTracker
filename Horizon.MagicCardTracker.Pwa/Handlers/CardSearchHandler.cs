
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
        private readonly IScryfallClientFactory _scryfallClientFactory;
        private readonly IMapper _mapper;

        public CardSearchHandler(
            IScryfallClientFactory scryfallClientFactory,
            IMapper mapper)
        {
            _scryfallClientFactory = scryfallClientFactory;
            _mapper = mapper;
        }
        public async Task<CardSearchResult> Handle(SearchCards request, CancellationToken cancellationToken)
        {
            var query = request.Query.Replace(" ", "+");
            var searchResult = await _scryfallClientFactory
                                        .Cards
                                        .SearchAsync(query,
                                                     null,
                                                     null,
                                                     null,
                                                     null,
                                                     include_multilingual: true,
                                                     null,
                                                     cancellationToken: cancellationToken);

            return new CardSearchResult
            {
                HasMoreResults = searchResult.Has_more,
                NumberOfMatchedCards = searchResult.Total_cards,
                Cards = _mapper.Map<IEnumerable<Contracts.Card>>(searchResult.Data)
            };
        }
    }
}