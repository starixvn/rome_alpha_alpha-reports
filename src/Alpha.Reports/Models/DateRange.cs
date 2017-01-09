namespace Alpha.Reports.Models
{
    using System;

    public interface DateRange
    {
        DateTime FromDate { get; }

        DateTime ToDate { get; }
    }

    public interface ThreeMonthCurrentOrThreeYearHistory : DateRange
    {
        bool IsHistoryReport { get; }
    }
}