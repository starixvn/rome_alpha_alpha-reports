namespace Alpha.Reports.Site.App_Start
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using Configurations;
    using Models;

    public class ReportSiteContext : SiteContext
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

        public IEnumerable<Product> Products
        {
            get
            {
                return SiteSettings.Products;
            }
        }
    }
}