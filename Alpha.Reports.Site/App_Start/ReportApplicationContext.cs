namespace Alpha.Reports.Site.App_Start
{
    using System.Collections.Generic;
    using Configurations;
    using Helpers;
    using Models;

    public class ReportApplicationContext : ApplicationContext
    {      
        public IEnumerable<Product> Products => SiteSettings.Products;        

        public string OldMainAppPath => AppPathHelper.GetAppPath(SiteSettings.OldMainAppPath, "~");
    }
}