namespace Alpha.Reports.Site.Helpers.Impl
{
    using Configurations;

    public class MicroSitesImpl : MicroSites
    {
        public string OldMainPath(string virtualPath) => AppPathHelper.GetAppPath(SiteSettings.OldMainAppPath, virtualPath);
    }
}
