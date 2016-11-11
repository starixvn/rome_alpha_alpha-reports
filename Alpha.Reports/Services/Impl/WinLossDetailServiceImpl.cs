namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLossDetail;
    using Fanex.Data.Repository;
    using Models;
    using Models.WinlossDetails;
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

        public ReportData<WinLossDetail> GetWinLossDetail(WinLossDetailParam param)
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

            var winLossDetailData = dynamicRepository.FetchMultiple<WinLossDetailDto, WinLossDetailDto>(criteria);

            return new ReportData<WinLossDetail>(
                winLossDetailData.Item1.Select(dto => new WinLossDetail(dto)),
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                winLossDetailData.Item2.Select(dto => new WinLossDetail(dto)).FirstOrDefault());
        }
    }
}
