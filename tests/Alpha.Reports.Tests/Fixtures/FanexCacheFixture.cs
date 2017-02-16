namespace Alpha.Reports.Tests.Fixtures
{
    using Fanex.Caching;
    using NSubstitute;

    public class FanexCacheFixture
    {
        public FanexCacheFixture()
        {
            CacheForTesting = Substitute.For<Cache>();
            ConfiguredCacheService.SwitchCacheStorage(CacheForTesting);
        }

        public Cache CacheForTesting { get; private set; }
    }
}
