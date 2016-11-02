namespace Alpha.Reports.Services.Impl
{
    using System;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

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
    }
}
