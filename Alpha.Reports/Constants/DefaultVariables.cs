namespace Alpha.Reports.Constants
{
    using System;
    using Fanex.Caching;

    public static class DefaultVariables
    {
        public const int PageSize = 1000;
        public const int PageIndex = 1;

        public const int PageSizeNoPaging = 0;
        public const int PageIndexNoPaging = 0;

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
