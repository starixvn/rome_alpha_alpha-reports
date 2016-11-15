namespace Alpha.Reports.Constants
{
    using System;
    using Fanex.Caching;

    public static class DefaultVariables
    {
        public const int PageSize = 2;
        public const int PageIndex = 1;

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
