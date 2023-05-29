namespace Alpha.Reports.Services
{
    using System;
    using System.Threading.Tasks;
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

        Task<PagedReportData<WinLossDetailModel>> GetWinLossDetailAsync(WinLossDetailParam param);
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
                productService.FilterProductIdsByCustomer(param.ViewerId, param.Site, param.CurrencyId, param.UserSelectedProductIds, isEnableSabaPromotion: param.IsEnableSabaPromotion);
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
                param.ProductDetailsXML,
                param.Username);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var winLossDetailData = dataAccess.GetData(criteria);

            return winLossDetailData;
        }

        public async Task<PagedReportData<WinLossDetailModel>> GetWinLossDetailAsync(WinLossDetailParam param)
        {
            var dayModel = await reportDateService.GetBalanceUpDayAndMoveBetsDayAsync();
            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                dayModel.BalanceUpDay,
                dayModel.MoveBetsDay,
                param.UserSelectedProductIds,
                param.ReportLevel,
                param.ProductDetailsXML,
                param.Username);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dataAccess.GetDataAsync(criteria);
        }
    }
}