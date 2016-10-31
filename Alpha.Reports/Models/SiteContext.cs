namespace Alpha.Reports.Models
{
    using System;
    using System.Collections.Generic;

    public interface SiteContext
    {
        //bool IsMbcSite { get; }
        DateTime ServerTime { get; }
        IEnumerable<Product> Products { get; }
    }
}
