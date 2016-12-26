namespace Alpha.Reports.Site.Tests.Helpers.Impl.StaticResources
{
    using System.Web.Configuration;
    using Adapters;
    using Site.Helpers;
    using Site.Helpers.Impl.StaticResources;
    using Xunit;

    public class BaseStaticResourcesTests
    {
        [Theory]
        [InlineData(true, "")]
        [InlineData(false, ".min")]
        public void MinText_DebugModeOrNot_ReturnsCorrectMinText(bool isDebug, string expectMinText)
        {
            // Arrange
            ConfigurationManagerAdapter.GetSection =
                (sectionName) =>
                {
                    var result = new CompilationSection();
                    result.Debug = isDebug;
                    return result;
                };
            BaseStaticResources.MinText = null;

            // Act
            var actualMinText = BaseStaticResources.MinText;

            // Assert
            Assert.Equal(expectMinText, actualMinText);
        }

        [Fact]
        public void StaticResourcesVersion_SetNotNullToStaticResourcesVersion_ReturnsSetStaticResourcesVersion()
        {
            // Arrange
            var expectedVersion = "1.0.0";
            BaseStaticResources.StaticResourcesVersion = expectedVersion;

            // Act
            var actualVersion = BaseStaticResources.StaticResourcesVersion;

            // Assert
            Assert.Equal(expectedVersion, actualVersion);
        }

        [Fact]
        public void StaticResourcesVersion_SetNullToStaticResourcesVersion_ReturnsAssemblyVersion()
        {
            // Arrange
            var expectedVersion = "1.0.0";
            AssemblyAdapter.Version = expectedVersion;
            BaseStaticResources.StaticResourcesVersion = null;

            // Act
            var actualVersion = BaseStaticResources.StaticResourcesVersion;

            // Assert
            Assert.Equal(expectedVersion, actualVersion);
        }
    }
}
