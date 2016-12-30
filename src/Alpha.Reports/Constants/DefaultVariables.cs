namespace Alpha.Reports.Constants
{
    using System;
    using Fanex.Caching;

    public static class DefaultVariables
    {
        public const int PageSize = 1000;
        public const int PageIndex = 1;

        public const int MaxRowExcelFile = 65536;

        public static CacheItemOptions CacheItemOptions => new CacheItemOptions
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        };
    }
}
