namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Linq;
    using System.Web;

    public class ReportsStyles : BaseStaticResources
    {
        public static IHtmlString Render(params string[] paths)
        {
            return System.Web.Optimization.Styles.Render(
                paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{MinText}.css?v={StaticResourcesVersion}"))
                    .ToArray());
        }
    }
}