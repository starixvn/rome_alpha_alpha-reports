namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using Configurations;
    using Extensions;
    using Models;

    public class ReportSiteContext : ApplicationContext
    {
        public DateTime ServerTime
        {
            get
            {
                var gmt = SiteSettings.DatabaseServerGMT;

                return gmt == null
                    ? DateTime.Now
                    : DateTime.Now.ToOffsetHour(gmt.Value);
            }
        }

        public IEnumerable<Product> Products => SiteSettings.Products;
    }
}