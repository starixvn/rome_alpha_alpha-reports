namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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

        public WinLossAnalysisChartServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<WinLossAnalysisChartModel> GetWinLossAnalysisChart(WinLossAnalysisChartParam param)
        {
            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds.Concat(param.UserSelectedExcludeProductIds));

            var winlossAnalysisChartData = Enumerable.Empty<WinLossAnalysisChartModel>();
            if (param.ToDate.GetMonthDiff(param.FromDate) >= 2)
            {
                var criteria = new WinLossAnalysisChartCriteria(
                                        param.CustId,
                                        param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                                        param.ToDate,
                                        reportDateService.GetBalanceUpDay(),
                                        reportDateService.GetMoveBetsDay(),
                                        validUserSelectedProductIds,
                                        param.CustLevel);

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