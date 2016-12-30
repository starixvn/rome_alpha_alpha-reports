namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;

    public static class ReportsStyles
    {
        public static IHtmlString Render(params string[] paths) => System.Web.Optimization.Styles.Render(
            paths.Select(path =>
            VirtualPathUtility.ToAbsolute($"{path}{BaseStaticResources.MinText}.css?v={BaseStaticResources.StaticResourcesVersion}"))
            .ToArray());
    }
}