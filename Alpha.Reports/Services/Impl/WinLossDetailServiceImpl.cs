namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Linq;
    using Criteria.WinLossDetail;
    using Fanex.Data.Repository;
    using Models;
    using Models.WinlossDetail;
    using Params;

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly IDynamicRepository dynamicRepository;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dynamicRepository = dynamicRepository;
        }

        public ReportData<T> GetWinLossDetail<T>(WinLossDetailParam param) where T : MemberWinLossDetail
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
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

            var winLossDetailData = dynamicRepository.FetchMultiple<T, T>(criteria);

            return new ReportData<T>(
                winLossDetailData.Item1,
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                winLossDetailData.Item2.FirstOrDefault());
        }
    }
}
