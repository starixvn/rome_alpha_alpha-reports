namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;

    public static class ReportsScripts
    {
        public static IHtmlString Render(params string[] paths)
        {
            return System.Web.Optimization.Scripts.Render(
                  paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{BaseStaticResources.MinText}.js?v={BaseStaticResources.StaticResourcesVersion}"))
                      .ToArray());
        }
    }
}