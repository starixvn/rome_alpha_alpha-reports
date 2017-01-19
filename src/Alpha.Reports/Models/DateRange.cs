namespace Alpha.Reports.Models
{
    using System;

    public enum DateRangeType
    {
        ThreeMonthCurrentOrThreeYearHistory
    }

    public class DateRange
    {
        public DateRange(
            DateRangeType dateRangeType,
            DateTime fromDate,
            DateTime toDate,
            bool? isHistoricReport = null)
        {
            RangeType = dateRangeType;
            FromDate = fromDate;
            ToDate = toDate;
            IsHistoricReport = isHistoricReport;
        }

        public DateRangeType RangeType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool? IsHistoricReport { get; }
    }
}