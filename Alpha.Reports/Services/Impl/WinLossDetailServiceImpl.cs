namespace Alpha.Reports.Services.Impl
{
    using System;
    using Criteria.WinLossDetail;
    using DataAccess;
    using Extensions;
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

        //public ReportData<MasterWinLossDetail> MasterWinLossDetail(WinLossDetailParam param)
        //{
        //    var criteria = CreateCriteria(param);

        //    return reportDataAccess.GetData<MasterWinLossDetail>(criteria);
        //}

        //public ReportData<AgentWinLossDetail> AgentWinLossDetail(WinLossDetailParam param)
        //{
        //   var criteria = CreateCriteria(param);

        //    return reportDataAccess.GetData<AgentWinLossDetail>(criteria);
        //}

        //public ReportData<MemberWinLossDetail> MemberWinLossDetail(WinLossDetailParam param)
        //{
        //    var criteria = CreateCriteria(param);           

        //    return reportDataAccess.GetData<MemberWinLossDetail>(criteria);
        //}

        private WinLossDetailCriteria CreateCriteria(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var criteria = new WinLossDetailCriteria(
             param.CustId,
             param.PageIndex,
             param.PageSize,
             param.FromDate.GetBeginningOfDate(),
             param.ToDate.GetEndingOfDate(),
             reportDateService.GetBalanceUpDay(),
             reportDateService.GetMoveBetsDay(),
             param.ProductIds,
             param.UserLevel);

            return criteria;
        }

        public ReportData<T> GetWinLossDetail<T>(WinLossDetailParam param) where T : MemberWinLossDetail
        {
            var criteria = CreateCriteria(param);

            return reportDataAccess.GetData<T>(criteria);
        }
    }
}
