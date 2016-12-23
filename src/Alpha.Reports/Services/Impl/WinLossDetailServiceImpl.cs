namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Linq;
    using Criteria.WinLossDetail;
    using DataAccesses;
    using Models;
    using Models.WinlossDetails;
    using Params;

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossDetailDataAccess dataAccess;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDetailDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
        }

        public ReportData<WinLossDetail> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!param.IsValid())
            {
                return new ReportData<WinLossDetail>(Enumerable.Empty<WinLossDetail>(), param.PageIndex, param.PageSize, 0, new WinLossDetail());
            }

            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds);

            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel);

            var winLossDetailData = dataAccess.GetData(criteria);

            return winLossDetailData;
        }
    }
}
