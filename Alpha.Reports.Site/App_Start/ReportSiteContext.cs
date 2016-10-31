namespace Alpha.Reports.Site.App_Start
{
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
                return DateTime.Now;
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