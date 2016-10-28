namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Configurations;

    public class AssetScripts : BaseStaticResources
    {
        public static IHtmlString Render(params string[] paths)
        {
            return System.Web.Optimization.Scripts.Render(paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{MinText}.js?v={StaticResourcesVersion}").ToArray());
        }
    }
}