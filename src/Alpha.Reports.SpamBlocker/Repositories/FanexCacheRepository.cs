namespace Alpha.Reports.SpamBlocker.Repositories
{
    using Alpha.Reports.SpamBlocker.Models;
    using Fanex.Caching;
    using System;

    public class FanexCacheRepository : ICacheRepository
    {
        private readonly CacheService cacheService;

        public FanexCacheRepository() : this(null)
        {
        }

        public FanexCacheRepository(CacheService cacheService)
        {
            this.cacheService = cacheService ?? new CacheServiceImpl();
        }

        public void Save(string id, ThrottleCounter throttleCounter, TimeSpan slidingExpiration)
        {
            cacheService.Set(
                id,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = slidingExpiration
                });
        }

        public bool Any(string id)
        {
            return cacheService.Contain(id);
        }

        public ThrottleCounter FirstOrDefault(string id)
        {
            return cacheService.Get<ThrottleCounter>(id);
        }

        public void Remove(string id)
        {
            cacheService.Remove(id);
        }
    }
}
