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
    }

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";

        private readonly IDynamicRepository dynamicRepository;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
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

            var threeMonthCurrentOrThreeYearHistory = dateRange as ThreeMonthCurrentOrThreeYearHistory;
            if (threeMonthCurrentOrThreeYearHistory != null)
            {
                return ValidateThreeMonthCurrentOrThreeYearHistoryRange(threeMonthCurrentOrThreeYearHistory);
            }

            return false;
        }

        private bool ValidateThreeMonthCurrentOrThreeYearHistoryRange(ThreeMonthCurrentOrThreeYearHistory dateRange)
        {
            if (dateRange.IsHistoryReport)
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
}