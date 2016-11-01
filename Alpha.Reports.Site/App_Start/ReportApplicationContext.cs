namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using Configurations;
    using Extensions;
    using Helpers;
    using Models;

    public class ReportApplicationContext : ApplicationContext
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

        public IEnumerable<ReportRange> ReportRanges => SiteSettings.ReportRanges;

        public string OldMainAppPath => AppPathHelper.GetAppPath(SiteSettings.OldMainAppPath, "~");
    }
}