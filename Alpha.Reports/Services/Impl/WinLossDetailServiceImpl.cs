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
        private readonly ReportDataAccess reportDataAccess;

        public WinLossDetailServiceImpl(ReportDateService reportDateService, ReportDataAccess reportDataAccess)
        {
            this.reportDateService = reportDateService;
            this.reportDataAccess = reportDataAccess;
        }

        public ReportData<T> GetWinLossDetail<T>(WinLossDetailParam param) where T : MemberWinLossDetail
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.ProductIds,
                param.UserLevel);

            return reportDataAccess.GetData<T>(criteria);
        }
    }
}
