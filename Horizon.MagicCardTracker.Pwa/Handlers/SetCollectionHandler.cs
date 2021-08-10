using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Horizon.MagicCardTracker.Pwa.Cache;
using Horizon.MagicCardTracker.Pwa.Queries;
using Horizon.MagicCardTracker.ScryfallClient;
using Horizon.MagicCardTracker.Storage;
using MediatR;

using Set = Horizon.MagicCardTracker.Pwa.Models.Set;

namespace Horizon.MagicCardTracker.Pwa.Handlers
{
    internal class SetCollectionHandler : IRequestHandler<GetCollectedSets, IEnumerable<Set>>
    {
        private readonly ICardLibrary _cardLibrary;
        private readonly IScryfallClientFactory _scryfallClientFactory;
        private readonly IObjectCache _cache;
        private readonly IMapper _mapper;

        public SetCollectionHandler(
            ICardLibrary cardLibrary,
            IScryfallClientFactory scryfallClientFactory,
            IObjectCache cache,
            IMapper mapper)
        {
            _cardLibrary = cardLibrary;
            _scryfallClientFactory = scryfallClientFactory;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Set>> Handle(
            GetCollectedSets request, 
            CancellationToken cancellationToken)
        {
            var collection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);
            var cardsBySet = collection.GroupBy(c => c.SetCode);
            var collectedSets = await GetCommonSetsAsync(cancellationToken);

            foreach (var setCode in cardsBySet)
            {
                var relatedSet = collectedSets.FirstOrDefault(s => s.Code == setCode.Key);
                if (relatedSet != null)
                {
                    relatedSet.NumberOfCollectedCards = setCode.Count();
                }
            }

            return collectedSets;
        }

        private async Task<List<Set>> GetCommonSetsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _cache.LookupObject<List<Set>>(KnownCacheKeys.Sets, cancellationToken);
            }
            catch (CacheMissException)
            {
                var sets = await _scryfallClientFactory.Sets.GetAllAsync(cancellationToken);
                var filteredSets = sets.Data
                                       .Where(s => s.Set_type == Set_type.Core 
                                                || s.Set_type == Set_type.Expansion)
                                       .Where(s => s.Released_at < DateTimeOffset.Now 
                                                && s.Card_count > 0)
                                       .Select(s => _mapper.Map<Set>(s))
                                       .OrderByDescending(s => s.ReleaseDate)
                                       .ToList();
                await _cache.CacheObject(KnownCacheKeys.Sets, filteredSets, cancellationToken);

                return filteredSets;
            }
        }
    }
}
