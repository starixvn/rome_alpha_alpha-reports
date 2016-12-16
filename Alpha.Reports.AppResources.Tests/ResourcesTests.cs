namespace Alpha.Reports.AppResources.Tests
{
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
