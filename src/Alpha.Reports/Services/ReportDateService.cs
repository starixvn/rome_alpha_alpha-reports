namespace Alpha.Reports.Services
{
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using System;
    using System.Collections.Generic;

    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();

        IEnumerable<TimeRange> GetTimeRangesToGetReportDataBetweenTables(DateTime fromDate, DateTime toDate);

        string GetStoreMappingSuffix(DateTime fromDate);
    }

    public class ReportDateServiceImpl : ReportDateService
    {
        public const string BetTrans14SPSuffix = "_14";
        public const string WarehouseSPSuffix = "_WH";

        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public static ReportDateService Instance { get; set; }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
        {
            var balanceUpDayAndMoveBetsDay = cacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () =>
                {
                    var reportDates = dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(
                        new GetBalanceUpDayAndMoveBetsDayCriteria());

                    return reportDates;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));

            return balanceUpDayAndMoveBetsDay;
        }

        public IEnumerable<TimeRange> GetTimeRangesToGetReportDataBetweenTables(DateTime fromDate, DateTime toDate)
        {
            var balanceUpDate = GetBalanceUpDay();
            var moveBetsDate = GetMoveBetsDay();

            var eventDates = new List<DateTime> { balanceUpDate.AddDays(-1), moveBetsDate };
            var result = new List<TimeRange>();

            foreach (var eventDate in eventDates)
            {
                if (fromDate <= eventDate && toDate <= eventDate)
                {
                    // No need to cut range any more
                    break;
                }

                if (fromDate <= eventDate)
                {
                    // Data between 2+ places
                    // Cut range: [From, Event Date][Event Date + 1, ToDate]
                    result.Add(new TimeRange(fromDate, eventDate));
                    fromDate = eventDate.AddDays(1);
                }
            }

            result.Add(new TimeRange(fromDate, toDate));

            return result;
        }

        public string GetStoreMappingSuffix(DateTime fromDate)
            => fromDate > GetMoveBetsDay()
            ? string.Empty
            : fromDate >= GetBalanceUpDay()
            ? BetTrans14SPSuffix
            : WarehouseSPSuffix;
    }
}