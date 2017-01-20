namespace Alpha.Reports.Services
{
    using System;
    using Criteria;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();

        bool ValidateDateRange(DateRange dateRange);

        DateTime GetMinDate(DateRangeType dateRangeType, bool? isHistoricReport);
    }

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";

        private readonly IDynamicRepository dynamicRepository;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public static void RegisterReportDateRangeTypes()
        {
            // ThreeMonthCurrentOrThreeYearHistory means user cannot view report before 3 months in current report
            // and 3 years in historic report
            WinLossServiceImpl.DateRangeType = DateRangeType.ThreeMonthCurrentOrThreeYearHistory;
            WinLossDetailServiceImpl.DateRangeType = DateRangeType.ThreeMonthCurrentOrThreeYearHistory;

            // More report types will be declared here.
        }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
        {
            var balanceUpDayAndMoveBetsDay = ConfiguredCacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () =>
                {
                    var reportDates = dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(
                        new GetBalanceUpDayAndMoveBetsDayCriteria());

                    return reportDates;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return balanceUpDayAndMoveBetsDay;
        }

        public bool ValidateDateRange(DateRange dateRange)
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

        public DateTime GetMinDate(DateRangeType dateRangeType, bool? isHistoricReport)
        {
            switch (dateRangeType)
            {
                case DateRangeType.ThreeMonthCurrentOrThreeYearHistory:
                    return GetThreeMonthCurrentOrThreeYearHistoryMinDate(isHistoricReport.GetValueOrDefault());

                default:
                    return DateTimeManager.Now;
            }
        }

        private static DateTime GetThreeMonthCurrentOrThreeYearHistoryMinDate(bool isHistoricReport)
            => isHistoricReport
                ? DateTimeManager.Now.AddYears(-3).FirstDayOfMonth()
                : DateTimeManager.Now.AddMonths(-3).FirstDayOfMonth();
    }
}