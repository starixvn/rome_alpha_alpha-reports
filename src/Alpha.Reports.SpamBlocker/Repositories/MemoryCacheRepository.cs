namespace Alpha.Reports.SpamBlocker.Repositories
{
    using Alpha.Reports.SpamBlocker.Models;
    using System;
    using System.Runtime.Caching;

    public class MemoryCacheRepository : ICacheRepository
    {
        private static ObjectCache memCache = new MemoryCache("SpamBlocker");

        public void Save(string id, ThrottleCounter throttleCounter, TimeSpan slidingExpiration)
        {
            if (memCache[id] != null)
            {
                memCache[id] = throttleCounter;
            }
            else
            {
                memCache.Add(
                    id,
                    throttleCounter, new CacheItemPolicy
                    {
                        SlidingExpiration = slidingExpiration
                    });
            }
        }

        public bool Any(string id)
        {
            return memCache[id] != null;
        }

        public ThrottleCounter FirstOrDefault(string id)
        {
            return (ThrottleCounter)memCache[id];
        }

        public void Remove(string id)
        {
            memCache.Remove(id);
        }
    }
}
