namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Criteria.WinLossAnalysisChart;
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
                productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds);

            var criteria = new WinLossAnalysisChartCriteria(
                                    param.CustId,
                                    param.FromDate,
                                    param.ToDate,
                                    reportDateService.GetBalanceUpDay(),
                                    reportDateService.GetMoveBetsDay(),
                                    validUserSelectedProductIds,
                                    param.CustLevel);

            var winlossAnalysisChartData = dynamicRepository.Fetch<WinLossAnalysisChartModel>(criteria);

            return winlossAnalysisChartData;
        }
    }
}