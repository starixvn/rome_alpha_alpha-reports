namespace Alpha.Reports.Site.Tests.Helpers.Impl.StaticResources
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using Adapters;
    using Alpha.Reports.Site.Configurations;
    using Site.Helpers.Impl.StaticResources;
    using Xunit;

    public class AssetStylesTests
    {
        public AssetStylesTests()
        {
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["AssetAppPath"] = "AssetAppPath",
                ["AssetsApp:StaticResourcesVersion"] = "444"
            };

            BaseStaticResources.StaticResourcesVersion = "StaticResourcesVersion";
            BaseStaticResources.MinText = ".MinText";
            WebOptimizationAdapter.FuncRenderStyles = (paths) =>
            {
                var scriptPaths = paths.Select(path => "<link href=\"/" + path + "\"></link>");

                return new HtmlString(string.Join(string.Empty, scriptPaths));
            };
        }

        [Fact]
        public void Render_Always_ReturnsCorrectRenderedPaths()
        {
            // Arrange
            var paths = new[] { "/Path" };
            var expectedPaths = "<link href=\"/AssetAppPath/Path.MinText.css?v=StaticResourcesVersion\"></link>";

            // Act
            var actualRenderedPaths = AssetStyles.Render(paths).ToString();

            // Assert
            Assert.Equal(expectedPaths, actualRenderedPaths);
        }

        [Fact]
        public void RenderCommon_Always_ReturnsCorrectRenderedPaths()
        {
            // Arrange
            var paths = new[] { "/Path" };
            var expectedPaths = "<link href=\"/AssetAppPath/Path.MinText.css?v=444\"></link>";

            // Act
            var actualRenderedPaths = AssetStyles.RenderCommon(paths).ToString();

            // Assert
            Assert.Equal(expectedPaths, actualRenderedPaths);
        }
    }
}
