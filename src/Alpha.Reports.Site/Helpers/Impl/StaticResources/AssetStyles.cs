namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Configurations;

    public static class AssetStyles
    {
        public static IHtmlString Render(params string[] paths) => System.Web.Optimization.Styles.Render(
                paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{BaseStaticResources.MinText}.css?v={BaseStaticResources.StaticResourcesVersion}").ToArray());

        public static IHtmlString RenderCommon(params string[] paths) => System.Web.Optimization.Styles.Render(
                paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{BaseStaticResources.MinText}.css?v={SiteSettings.StaticResourcesVersion}").ToArray());
    }
}