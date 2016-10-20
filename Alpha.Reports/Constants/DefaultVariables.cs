namespace Alpha.Reports.Constants
{
    using System;
    using Fanex.Caching;

    public class DefaultVariables
    {
        public const int PageSize = 50;

        public static CacheItemOptions CacheItemOptions
        {
            get
            {
                return new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
            }
        }
    }
}
