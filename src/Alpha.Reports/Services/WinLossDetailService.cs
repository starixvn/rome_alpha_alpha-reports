namespace Alpha.Reports.Services
{
    using System;
    using Alpha.PartialUM.Services;
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
        private readonly ProductDetailService productDetailService;
        private readonly WinLossDetailDataAccess dataAccess;
        private readonly IPartialUMService partialUMService;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailDataAccess dataAccess,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.dataAccess = dataAccess;
            this.partialUMService = partialUMService;
        }

        public PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.CustId, param.Site, param.CurrencyId, param.UserSelectedProductIds, isEnableSabaPromotion: param.IsEnableSabaPromotion);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, param.UserSelectedResourceIds);

            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel,
                param.ProductDetailsXML);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var winLossDetailData = dataAccess.GetData(criteria);

            return winLossDetailData;
        }
    }
}