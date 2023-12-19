namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Criteria.WinLossAnalysisChart;
    using Extensions;
    using Fanex.Data.Repository;
    using Models.WinLossAnalysisChart;
    using Params;

    public interface WinLossAnalysisChartService
    {
        IEnumerable<WinLossAnalysisChartModel> GetWinLossAnalysisChart(WinLossAnalysisChartParam param);
    }

    public class WinLossAnalysisChartServiceImpl : WinLossAnalysisChartService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private const int DefaultLoadingMonths = 2;

        public WinLossAnalysisChartServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<WinLossAnalysisChartModel> GetWinLossAnalysisChart(WinLossAnalysisChartParam param)
        {
            var validUserSelectedProductIds =
                productService.FilterProductIds(param.Site, param.CurrencyId, param.UserSelectedProductIds);

            var winlossAnalysisChartData = Enumerable.Empty<WinLossAnalysisChartModel>();
            if (param.ToDate.GetMonthDiff(param.FromDate) >= DefaultLoadingMonths)
            {
                var criteria = new WinLossAnalysisChartCriteria(
                                        param.CustId,
                                        param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                                        param.ToDate,
                                        reportDateService.GetBalanceUpDay(),
                                        reportDateService.GetMoveBetsDay(),
                                        validUserSelectedProductIds,
                                        param.CustLevel);

                partialUMService.ThrowIfUM(criteria.GetSettingKey());

                winlossAnalysisChartData = dynamicRepository.Fetch<WinLossAnalysisChartModel>(criteria);
                winlossAnalysisChartData = AddEmptyWinlossInLimitedDate(winlossAnalysisChartData, param);
            }

            return winlossAnalysisChartData;
        }

        private static IEnumerable<WinLossAnalysisChartModel> AddEmptyWinlossInLimitedDate(
            IEnumerable<WinLossAnalysisChartModel> winloss, WinLossAnalysisChartParam param)
        {
            var winlossToList = winloss.ToList();
            for (var date = param.ToDate; date >= param.FromDate; date = date.AddMonths(-1))
            {
                var winlossDate = date.ToString("yyyy-MM");
                var isNotExisted = !winlossToList.Any(i => i.WinLossDate.Equals(winlossDate));
                if (isNotExisted)
                {
                    winlossToList.Insert(0, new WinLossAnalysisChartModel(winlossDate, 0, 0));
                }
            }

            return winlossToList.AsEnumerable<WinLossAnalysisChartModel>();
        }
    }
}