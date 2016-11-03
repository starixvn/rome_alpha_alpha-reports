namespace Alpha.Reports.Services.Impl
{
    using System;
    using Criteria.WinLossDetail;
    using DataAccess;
    using Helpers;
    using Models;
    using Models.WinlossDetail;
    using Params;

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly WinLossDetailDataAccess reportDataAccess;

        public WinLossDetailServiceImpl(ReportDateService reportDateService, WinLossDetailDataAccess reportDataAccess)
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
                param.ViewerLevel);

            var rawData = reportDataAccess.GetData<T>(criteria);
            return new ReportData<T>(
                IEnumerableHelper.Sort(rawData.DataSource, param.SortedColumn, param.SortDirection),
                criteria.PageIndex,
                criteria.PageSize,
                rawData.TotalRecords,
                rawData.Total);
        }
    }
}
