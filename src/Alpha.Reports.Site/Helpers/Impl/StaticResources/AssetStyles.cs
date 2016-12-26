namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Adapters;
    using Configurations;

    public static class AssetStyles
    {
        public static IHtmlString Render(params string[] paths)
        {
            return WebOptimizationAdapter.FuncRenderStyles(
                paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{BaseStaticResources.MinText}.css?v={BaseStaticResources.StaticResourcesVersion}").ToArray());
        }

        public static IHtmlString RenderCommon(params string[] paths)
        {
            return WebOptimizationAdapter.FuncRenderStyles(
                paths.Select(path => $"{SiteSettings.AssetAppPath}{path}{BaseStaticResources.MinText}.css?v={SiteSettings.StaticResourcesVersion}").ToArray());
        }
    }
}