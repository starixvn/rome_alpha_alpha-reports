namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Linq;
    using Criteria;
    using Enums;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Alpha.Reports.Extensions;

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private const string ReportDateRangeCacheKey = "Alpha.Reports.ReportDateRange";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ApplicationContext applicationContext;

        public ReportDateServiceImpl(
            IDynamicRepository dynamicRepository = null,
            ApplicationContext applicationContext = null)
        {
            this.dynamicRepository = dynamicRepository;
            this.applicationContext = applicationContext;
        }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
        {
            var balanceUpDayAndMoveBetsDay = CacheService.GetOrSet(
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

        public DateRange GetDateRangeLimit(ReportDateRangeParam param)
            => new DateRange(GetMinDate(param), maxDate: DateTime.MaxValue);

        private DateTime GetMinDate(ReportDateRangeParam param)
        {
            var rangeType = GetReportRangeType(param);

            switch (rangeType)
            {
                case ReportTimeRangeType.BalanceUp:
                    return GetBalanceUpDay();

                case ReportTimeRangeType.FromLast3Months:
                    return applicationContext.ServerTime.AddMonths(-3).FirstDayOfMonth();

                case ReportTimeRangeType.FromLast18Months:
                    return applicationContext.ServerTime.AddMonths(-18).FirstDayOfMonth();

                case ReportTimeRangeType.FromLast3Years:
                    return applicationContext.ServerTime.AddYears(-3).FirstDayOfMonth();

                case ReportTimeRangeType.Unknown:
                default:
                    return applicationContext.ServerTime;
            }
        }

        private ReportTimeRangeType GetReportRangeType(ReportDateRangeParam param)
        {
            byte rangeType = 0;
            var reportPath = param.ReportPath + (param.IsHistoricReport ? "Historic" : string.Empty);

            foreach (var reportRange in param.ReportRangeSettings)
            {
                if (reportRange.Reports
                    .Where(report => string.Equals(report, reportPath, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault() != null)
                {
                    rangeType = reportRange.Range;
                    break;
                }
            }

            var reportTimeRangeType = ReportTimeRangeType.Unknown;
            Enum.TryParse(rangeType.ToString(), out reportTimeRangeType);

            return reportTimeRangeType;
        }
    }
}
