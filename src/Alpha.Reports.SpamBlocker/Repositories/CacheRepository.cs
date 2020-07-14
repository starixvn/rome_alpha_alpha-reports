namespace Alpha.Reports.SpamBlocker.Repositories
{
    using Alpha.Reports.SpamBlocker.Models;
    using System;
    using System.Web;
    using System.Web.Caching;

    public class CacheRepository : ICacheRepository
    {
        public void Save(string id, ThrottleCounter throttleCounter, TimeSpan slidingExpiration)
        {
            if (HttpContext.Current.Cache[id] != null)
            {
                HttpContext.Current.Cache[id] = throttleCounter;
            }
            else
            {
                HttpContext.Current.Cache.Add(
                    id,
                    throttleCounter,
                    null,
                    Cache.NoAbsoluteExpiration,
                    slidingExpiration,
                    CacheItemPriority.Low,
                    null);
            }
        }

        public bool Any(string id)
        {
            return HttpContext.Current.Cache[id] != null;
        }

        public ThrottleCounter FirstOrDefault(string id)
        {
            return (ThrottleCounter)HttpContext.Current.Cache[id];
        }

        public void Remove(string id)
        {
            HttpContext.Current.Cache.Remove(id);
        }
    }
}
