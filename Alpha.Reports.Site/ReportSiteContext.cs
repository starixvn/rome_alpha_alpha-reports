namespace Alpha.Reports.Site
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Configurations;

    public class ReportSiteContext : SiteContext
    {
        //public bool IsMbcSite => string.IsNullOrWhiteSpace(SiteSettings.GetConfigValue("IsMbcSite")) ? 

        public IEnumerable<Product> Products => SiteSettings.Products;

        public DateTime ServerTime => DateTime.Now;
    }
}