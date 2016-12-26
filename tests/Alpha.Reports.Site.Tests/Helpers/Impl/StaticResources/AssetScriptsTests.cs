namespace Alpha.Reports.Site.Tests.Helpers.Impl.StaticResources
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using Adapters;
    using Extensions;
    using Site.Configurations;
    using Site.Helpers;
    using Site.Helpers.Impl.StaticResources;
    using Xunit;

    public class AssetScriptsTests
    {
        public AssetScriptsTests()
        {
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["AssetAppPath"] = "AssetAppPath",
                ["ContentApp:StaticResourcesVersion"] = "444"
            };

            BaseStaticResources.StaticResourcesVersion = "StaticResourcesVersion";
            BaseStaticResources.MinText = ".MinText";
            WebOptimizationAdapter.FuncRenderScripts = (paths) =>
            {
                var scriptPaths = paths.Select(path => "<script src=\"/" + path + "\"></script>");

                return new HtmlString(string.Join(string.Empty, scriptPaths));
            };
        }

        [Fact]
        public void Render_Always_ReturnsCorrectRenderedPaths()
        {
            // Arrange
            var paths = new[] { "/Path" };
            var expectedPaths = "<script src=\"/AssetAppPath/Path.MinText.js?v=StaticResourcesVersion\"></script>";

            // Act
            var actualRenderedPaths = AssetScripts.Render(paths).ToString();

            // Assert
            Assert.Equal(expectedPaths, actualRenderedPaths);
        }

        [Fact]
        public void RenderCommon_Always_ReturnsCorrectRenderedPaths()
        {
            // Arrange
            var paths = new[] { "/Path" };
            var expectedPaths = "<script src=\"/AssetAppPath/Path.MinText.js?v=444\"></script>";

            // Act
            var actualRenderedPaths = AssetScripts.RenderCommon(paths).ToString();

            // Assert
            Assert.Equal(expectedPaths, actualRenderedPaths);
        }
    }
}
