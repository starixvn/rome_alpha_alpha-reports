namespace Alpha.Reports.Tests.TestFixtures
{
    using Xunit;

    [CollectionDefinition("ReportCollection")]
    public class ReportCollection : ICollectionFixture<TestContextFixture>
    {
    }
}