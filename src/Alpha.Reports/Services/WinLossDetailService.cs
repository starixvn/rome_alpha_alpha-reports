namespace Alpha.Reports.Services
{
    using System;
    using System.Linq;
    using Criteria.WinLossDetail;
    using DataAccesses;
    using Extensions;
    using Models;
    using Models.WinLossDetail;
    using Params;

    public interface WinLossDetailService
    {
        PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);
    }

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

        public PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.CustId, param.Site, param.CurrencyId, param.UserSelectedProductIds);

            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
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