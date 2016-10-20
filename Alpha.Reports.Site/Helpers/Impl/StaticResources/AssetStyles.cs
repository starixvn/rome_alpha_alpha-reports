namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;
    using Configurations;

    public class AssetStyles : BaseStaticResources
    {
        public static IHtmlString Render(params string[] paths)
        {
            return System.Web.Optimization.Styles.Render(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{MinText}.css?v={StaticResourcesVersion}").ToArray());
        }
    }
}