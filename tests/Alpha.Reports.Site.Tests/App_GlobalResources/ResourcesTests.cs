namespace Alpha.Reports.Site.Tests.App_GlobalResources
{
    using Site.App_GlobalResources;
    using Xunit;

    public class ResourcesTests
    {
        [Fact]
        public void Create_Always_ReturnsNotNull()
        {
            // Act
            var resources = new Resources();

            // Assert
            Assert.NotNull(resources);
        }
    }
}