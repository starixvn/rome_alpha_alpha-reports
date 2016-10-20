namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;

    public class ReportsScripts : BaseStaticResources
    {
        public static IHtmlString Render(params string[] paths)
        {
            return System.Web.Optimization.Scripts.Render(
                  paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{MinText}.js?v={StaticResourcesVersion}"))
                      .ToArray());
        }
    }
}