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

            var reportDate = reportDateService.GetBalanceUpDayAndMoveBetsDay();
            var validUserSelectedProductIds =
                productService.FilterProductIds(param.Site, param.CurrencyId, param.UserSelectedProductIds);
            var criteria = GetWinLossDetailCriteria(
                param,
                reportDate,
                validUserSelectedProductIds == null ? string.Empty : validUserSelectedProductIds.JoinWithComma());
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return dataAccess.GetData(criteria);
        }

        public async Task<PagedReportData<WinLossDetailModel>> GetWinLossDetailAsync(WinLossDetailParam param)
        {
            var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDayAsync();
            var criteria = GetWinLossDetailCriteria(
                param,
                reportDate,
                param.UserSelectedProductIds == null ? string.Empty : param.UserSelectedProductIds.JoinWithComma());
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
                param.LevelType,
                subfix);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await repository.FetchAsync<WinLossDetailWeekData>(criteria);
        }

        private static WinLossDetailMemberCriteria GetWinLossDetailCriteria(
            WinLossDetailParam param,
            BalanceUpDayAndMoveBetsDay reportDate,
            string validUserSelectedProductIds)
        {
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            if (param.ReportLevel.IsAgentLevel())
            {
                return new WinLossDetailMemberCriteria(
                    param.CustId,
                    param.PageIndex,
                    param.PageSize,
                    param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                    param.ToDate,
                    reportDate.BalanceUpDay,
                    reportDate.MoveBetsDay,
                    validUserSelectedProductIds,
                    param.ReportLevel,
                    param.ProductDetailsXML,
                    param.Username,
                    param.IsReward,
                    subfix);
            }

            if (param.ReportLevel.IsMasterLevel())
            {
                return new WinLossDetailAgentCriteria(
                    param.CustId,
                    param.PageIndex,
                    param.PageSize,
                    param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                    param.ToDate,
                    reportDate.BalanceUpDay,
                    reportDate.MoveBetsDay,
                    validUserSelectedProductIds,
                    param.ReportLevel,
                    param.ProductDetailsXML,
                    param.Username,
                    param.IsReward,
                    subfix,
                    param.LevelType);
            }

            return new WinLossDetailMasterCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDate.BalanceUpDay,
                reportDate.MoveBetsDay,
                validUserSelectedProductIds,
                param.ReportLevel,
                param.ProductDetailsXML,
                param.Username,
                param.IsReward,
                subfix,
                param.LevelType);
        }
    }
}