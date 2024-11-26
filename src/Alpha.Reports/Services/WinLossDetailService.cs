namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Reports.Helpers;
    using AlphaPlus.PartialUM.Services;
    using Criteria.WinLossDetail;
    using DataAccesses;
    using Extensions;
    using Fanex.Data.Repository;
    using Models;
    using Models.WinLossDetail;
    using Params;

    public interface WinLossDetailService
    {
        PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);

        Task<PagedReportData<WinLossDetailModel>> GetWinLossDetailAsync(WinLossDetailParam param);

        Task<IEnumerable<WinLossDetailWeekData>> GetWinLossDetailWeekDataAsync(WinLossDetailWeekDataParam param);
    }

    public class WinLossDetailServiceImpl : WinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossDetailDataAccess dataAccess;
        private readonly IPartialUMService partialUMService;
        private readonly IDynamicRepository repository;

        public WinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDetailDataAccess dataAccess,
            IPartialUMService partialUMService,
            IDynamicRepository repository)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
            this.partialUMService = partialUMService;
            this.repository = repository;
        }

        public PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIds(param.Site, param.CurrencyId, param.UserSelectedProductIds);
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds == null ? string.Empty : validUserSelectedProductIds.JoinWithComma(),
                param.ReportLevel,
                param.ProductDetailsXML,
                param.Username,
                param.IsReward,
                subfix);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var winLossDetailData = dataAccess.GetData(criteria);

            return winLossDetailData;
        }

        public async Task<PagedReportData<WinLossDetailModel>> GetWinLossDetailAsync(WinLossDetailParam param)
        {
            var dayModel = await reportDateService.GetBalanceUpDayAndMoveBetsDayAsync();
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                dayModel.BalanceUpDay,
                dayModel.MoveBetsDay,
                param.UserSelectedProductIds == null ? string.Empty : param.UserSelectedProductIds.JoinWithComma(),
                param.ReportLevel,
                param.ProductDetailsXML,
                param.Username,
                param.IsReward,
                subfix);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dataAccess.GetDataAsync(criteria);
        }

        public async Task<IEnumerable<WinLossDetailWeekData>> GetWinLossDetailWeekDataAsync(WinLossDetailWeekDataParam param)
        {
            var dayModel = await reportDateService.GetBalanceUpDayAndMoveBetsDayAsync();
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new WinLossDetailWeekDataCriteria(
                param.ViewerId,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                dayModel.BalanceUpDay,
                dayModel.MoveBetsDay,
                param.UserSelectedProductIds == null ? string.Empty : param.UserSelectedProductIds.JoinWithComma(),
                param.ProductDetailsXML,
                param.Username,
                param.IsReward,
                subfix);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await repository.FetchAsync<WinLossDetailWeekData>(criteria);
        }
    }
}