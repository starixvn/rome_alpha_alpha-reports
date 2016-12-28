namespace Alpha.Reports.Tests.Collection
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("FanexCacheCollection")]
    public class FanexCacheCollection : ICollectionFixture<FanexCacheFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
