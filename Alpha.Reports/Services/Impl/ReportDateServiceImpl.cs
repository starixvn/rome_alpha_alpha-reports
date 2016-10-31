namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Linq;
    using Criteria;
    using DataAccess;
    using Enums;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Params;

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private const string ReportDateRangeCacheKey = "Alpha.Reports.ReportDateRange";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateAccess reportDateAccess;
        private readonly SiteContext siteContext;

        public ReportDateServiceImpl(
            IDynamicRepository dynamicRepository = null,
            ReportDateAccess reportDateAccess = null,
            SiteContext siteContext = null)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateAccess = reportDateAccess;
            this.siteContext = siteContext;
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

        public DateRange GetReportDateRange(ReportDateRangeParam param)
        {
            byte rangeType = GetReportRangeType(param);

            DateRange dateRange = CacheService.Get<DateRange>(ReportDateRangeCacheKey + rangeType);
            if (dateRange != null)
            {
                return dateRange;
            }

            ReportTimeRangeType reportTimeRangeType;

            if (!Enum.TryParse(rangeType.ToString(), out reportTimeRangeType) ||
                reportTimeRangeType == ReportTimeRangeType.Unknown)
            {
                dateRange = new DateRange(siteContext.ServerTime, siteContext.ServerTime);
            }
            else
            {
                dateRange = reportDateAccess.GetReportDateRange(new ReportDateRangeCriteria(rangeType));
                CacheService.Set(
                    ReportDateRangeCacheKey + rangeType,
                    dateRange,
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            }

            return dateRange;
        }

        private byte GetReportRangeType(ReportDateRangeParam param)
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

            return rangeType;
        }
    }
}
