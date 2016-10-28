namespace Alpha.Reports.Services.Impl
{
    using Criteria.WinLossDetail;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Models.WinlossDetail;
    using Params;

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public ReportData<MemberWinlossDetail> MemberWinLossDetail(WinLossDetailParam param)
        {
            var criteria = new MemberWinLossDetailCriteria(
                param.CustId,
                DateTimeHelper.GetBeginningOfDate(param.FromDate),
                DateTimeHelper.GetEndingOfDate(param.ToDate),
                reportDateService.GetBalanceUpDay(),
                string.Join(",", param.ProductIds),
                param.PageSize,
                param.PageIndex);

            var rawData = dynamicRepository.Fetch<MemberWinlossDetail>(criteria);

            return new ReportData<MemberWinlossDetail>(
                rawData,
                param.PageIndex,
                criteria.TotalRows,
                param.FromDate,
                param.ToDate);
        }
    }
}
