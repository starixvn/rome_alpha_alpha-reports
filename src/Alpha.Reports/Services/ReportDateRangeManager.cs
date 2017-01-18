namespace Alpha.Reports.Services
{
    using System;
    using Extensions;

    public enum DateRangeType
    {
        ThreeMonthCurrentOrThreeYearHistory
    }

    public static class ReportDateRangeManager
    {
        public static Func<DateRange, bool> ValidateDateRangeFunc { get; internal set; } = ValidateDateRange;

        public static void Init()
        {
            // ThreeMonthCurrentOrThreeYearHistory means user cannot view report before 3 months in current report
            // and 3 years in historic report
            WinLossServiceImpl.DateRangeType = DateRangeType.ThreeMonthCurrentOrThreeYearHistory;
            WinLossDetailServiceImpl.DateRangeType = DateRangeType.ThreeMonthCurrentOrThreeYearHistory;
        }

        internal static bool ValidateDateRange(DateRange dateRange)
        {
            if (dateRange == null)
            {
                throw new ArgumentNullException(nameof(dateRange));
            }

            switch (dateRange.RangeType)
            {
                case DateRangeType.ThreeMonthCurrentOrThreeYearHistory:
                    return ValidateThreeMonthCurrentOrThreeYearHistoryRange(dateRange);

                default:
                    return false;
            }
        }

        private static bool ValidateThreeMonthCurrentOrThreeYearHistoryRange(DateRange dateRange)
        {
            if (dateRange.IsHistoricReport.GetValueOrDefault())
            {
                return DateTimeManager.Now.AddYears(-3).FirstDayOfYear() <= dateRange.FromDate
                    && dateRange.FromDate <= dateRange.ToDate
                    && dateRange.ToDate.Year == dateRange.FromDate.Year
                    && dateRange.ToDate.Month == dateRange.FromDate.Month;
            }

            return DateTimeManager.Now.AddMonths(-3).FirstDayOfMonth() <= dateRange.FromDate
                   && dateRange.FromDate <= dateRange.ToDate;
        }
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