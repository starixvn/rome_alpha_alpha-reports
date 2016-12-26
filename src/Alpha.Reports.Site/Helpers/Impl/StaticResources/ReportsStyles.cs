namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Adapters;

    public static class ReportsStyles
    {
        public static IHtmlString Render(params string[] paths)
        {
            return WebOptimizationAdapter.FuncRenderStyles(
                paths.Select(path =>
                    VirtualPathUtilityAdapter.FuncToAbsolute($"{path}{BaseStaticResources.MinText}.css?v={BaseStaticResources.StaticResourcesVersion}"))
                    .ToArray());
        }
    }
}