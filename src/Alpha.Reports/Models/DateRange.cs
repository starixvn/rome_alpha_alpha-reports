namespace Alpha.Reports.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Reviewed")]
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