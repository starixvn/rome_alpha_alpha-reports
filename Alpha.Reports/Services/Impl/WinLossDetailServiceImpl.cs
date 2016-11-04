namespace Alpha.Reports.Services.Impl
{
    using System;
    using Criteria.WinLossDetail;
    using DataAccess;
    using Models;
    using Models.WinlossDetail;
    using Params;

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossDetailDataAccess reportDataAccess;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDetailDataAccess reportDataAccess)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.reportDataAccess = reportDataAccess;
        }

        public ReportData<T> GetWinLossDetail<T>(WinLossDetailParam param) where T : MemberWinLossDetail
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                string.Join(
                    ",",
                    productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds));

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

            return reportDataAccess.GetData<T>(criteria);            
        }
    }
}
