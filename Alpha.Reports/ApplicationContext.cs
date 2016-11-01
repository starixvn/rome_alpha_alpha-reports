namespace Alpha.Reports
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface ApplicationContext
    {
        DateTime ServerTime { get; }

        IEnumerable<Product> Products { get; }

        IEnumerable<ReportRange> ReportRanges { get; }

        string OldMainAppPath { get; }
    }
}
