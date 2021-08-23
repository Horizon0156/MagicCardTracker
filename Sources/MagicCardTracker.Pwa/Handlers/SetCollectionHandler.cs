using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MagicCardTracker.Pwa.Cache;
using MagicCardTracker.Pwa.Commands;
using MagicCardTracker.Pwa.Notifications;
using MagicCardTracker.Pwa.Queries;
using MagicCardTracker.ScryfallClient;
using MagicCardTracker.Storage;
using MediatR;

using Set = MagicCardTracker.Pwa.Models.Set;

namespace MagicCardTracker.Pwa.Handlers
{
    internal class SetCollectionHandler : 
        IRequestHandler<GetSetsWithCollectionInfo, IEnumerable<Set>>,
        IRequestHandler<UpdateSets>
    {
        private readonly ICardLibrary _cardLibrary;
        private readonly IScryfallClientFactory _scryfallClientFactory;
        private readonly IObjectCache _cache;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public SetCollectionHandler(
            ICardLibrary cardLibrary,
            IScryfallClientFactory scryfallClientFactory,
            IObjectCache cache,
            IMapper mapper,
            INotificationService notificationService)
        {
            _cardLibrary = cardLibrary;
            _scryfallClientFactory = scryfallClientFactory;
            _cache = cache;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Set>> Handle(
            GetSetsWithCollectionInfo request, 
            CancellationToken cancellationToken)
        {
            var collection = await _cardLibrary.GetCollectedCardsAsync(cancellationToken);
            var cardsBySet = collection.GroupBy(c => c.SetCode);
            var collectedSets = await GetCommonSetsFromCacheAsync(cancellationToken);

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

        public async Task<Unit> Handle(UpdateSets request, CancellationToken cancellationToken)
        {
            var sets = await GetCommonSetsAsync(cancellationToken);
            _notificationService.SendNotification(new Notification($"Updated information for {sets.Count} sets"));

            return Unit.Value;
        }

        private async Task<List<Set>> GetCommonSetsFromCacheAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                return await _cache.LookupObject<List<Set>>(KnownCacheKeys.Sets, cancellationToken);
            }
            catch (CacheMissException)
            {
                return await GetCommonSetsAsync(cancellationToken);
            }
        }

        private async Task<List<Set>> GetCommonSetsAsync(CancellationToken cancellationToken)
        {
            var sets = await _scryfallClientFactory.Sets.GetAllAsync(cancellationToken);
            var filteredSets = sets.Data
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
