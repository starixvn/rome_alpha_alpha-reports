namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Adapters;

    public static class ReportsScripts
    {
        public static IHtmlString Render(params string[] paths)
        {
            return WebOptimizationAdapter.FuncRenderScripts(
                  paths.Select(path => VirtualPathUtilityAdapter.FuncToAbsolute($"{path}{BaseStaticResources.MinText}.js?v={BaseStaticResources.StaticResourcesVersion}"))
                      .ToArray());
        }
    }
}