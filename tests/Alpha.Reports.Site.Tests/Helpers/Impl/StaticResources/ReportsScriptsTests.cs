namespace Alpha.Reports.Site.Tests.Helpers.Impl.StaticResources
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using Adapters;
    using Site.Configurations;
    using Site.Helpers;
    using Site.Helpers.Impl.StaticResources;
    using Xunit;

    public class ReportsScriptsTests
    {
        public ReportsScriptsTests()
        {
            BaseStaticResources.StaticResourcesVersion = "StaticResourcesVersion";
            BaseStaticResources.MinText = ".MinText";
            WebOptimizationAdapter.FuncRenderScripts = (paths) =>
            {
                var scriptPaths = paths.Select(path => "<script src=\"/" + path + "\"></script>");

                return new HtmlString(string.Join(string.Empty, scriptPaths));
            };

            VirtualPathUtilityAdapter.FuncToAbsolute = (path) => $"Reports{path}";
        }

        [Fact]
        public void Render_Always_ReturnsCorrectRenderedPaths()
        {
            // Arrange
            var paths = new[] { "/Path" };
            var expectedPaths = "<script src=\"/Reports/Path.MinText.js?v=StaticResourcesVersion\"></script>";

            // Act
            var actualRenderedPaths = ReportsScripts.Render(paths).ToString();

            // Assert
            Assert.Equal(expectedPaths, actualRenderedPaths);
        }
    }
}
