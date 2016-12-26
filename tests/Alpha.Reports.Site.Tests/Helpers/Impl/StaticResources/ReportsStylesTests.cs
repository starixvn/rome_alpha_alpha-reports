namespace Alpha.Reports.Site.Tests.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Adapters;
    using Alpha.Reports.Site.Helpers.Impl.StaticResources;
    using Xunit;

    public class ReportsStylesTests
    {
        public ReportsStylesTests()
        {
            BaseStaticResources.StaticResourcesVersion = "StaticResourcesVersion";
            BaseStaticResources.MinText = ".MinText";
            WebOptimizationAdapter.FuncRenderStyles = (paths) =>
            {
                var scriptPaths = paths.Select(path => "<link href=\"/" + path + "\"></link>");

                return new HtmlString(string.Join(string.Empty, scriptPaths));
            };

            VirtualPathUtilityAdapter.FuncToAbsolute = (path) => $"Reports{path}";
        }

        [Fact]
        public void Render_Always_ReturnsCorrectRenderedPaths()
        {
            // Arrange
            var paths = new[] { "/Path" };
            var expectedPaths = "<link href=\"/Reports/Path.MinText.css?v=StaticResourcesVersion\"></link>";

            // Act
            var actualRenderedPaths = ReportsStyles.Render(paths).ToString();

            // Assert
            Assert.Equal(expectedPaths, actualRenderedPaths);
        }
    }
}
