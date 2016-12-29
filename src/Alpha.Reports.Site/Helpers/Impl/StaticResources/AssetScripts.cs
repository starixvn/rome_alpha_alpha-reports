namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Configurations;

    public static class AssetScripts
    {
        public static IHtmlString Render(params string[] paths)
        {
            return System.Web.Optimization.Scripts.Render(
                paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{BaseStaticResources.MinText}.js?v={BaseStaticResources.StaticResourcesVersion}").ToArray());
        }

        public static IHtmlString RenderCommon(params string[] paths)
        {
            return System.Web.Optimization.Scripts.Render(
                paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{BaseStaticResources.MinText}.js?v={SiteSettings.StaticResourcesVersion}").ToArray());
        }
    }
}