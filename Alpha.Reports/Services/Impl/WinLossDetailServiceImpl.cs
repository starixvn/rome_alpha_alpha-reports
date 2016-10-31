namespace Alpha.Reports.Services.Impl
{
    using System;
    using Criteria.WinLossDetail;
    using DataAccess;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Models.WinlossDetail;
    using Params;

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDataAccess reportDataAccess;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            ReportDataAccess reportDataAccess)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.reportDataAccess = reportDataAccess;
        }

        public ReportData<MasterWinLossDetail> MasterWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var criteria = new MasterWinLossDetailCriteria(
                param.CustId,
                DateTimeHelper.GetBeginningOfDate(param.FromDate),
                DateTimeHelper.GetEndingOfDate(param.ToDate),
                reportDateService.GetBalanceUpDay(),
                string.Join(",", param.ProductIds),
                param.PageSize,
                param.PageIndex);

            return reportDataAccess.GetData<MasterWinLossDetail>(criteria);
        }

        public ReportData<AgentWinLossDetail> AgentWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var criteria = new AgentWinLossDetailCriteria(
                param.CustId,
                DateTimeHelper.GetBeginningOfDate(param.FromDate),
                DateTimeHelper.GetEndingOfDate(param.ToDate),
                reportDateService.GetBalanceUpDay(),
                string.Join(",", param.ProductIds),
                param.PageSize,
                param.PageIndex);

            return reportDataAccess.GetData<AgentWinLossDetail>(criteria);
        }

        public ReportData<MemberWinLossDetail> MemberWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var criteria = new MemberWinLossDetailCriteria(
                param.CustId,
                DateTimeHelper.GetBeginningOfDate(param.FromDate),
                DateTimeHelper.GetEndingOfDate(param.ToDate),
                reportDateService.GetBalanceUpDay(),
                string.Join(",", param.ProductIds),
                param.PageSize,
                param.PageIndex);

            return reportDataAccess.GetData<MemberWinLossDetail>(criteria);
        }
    }
}
