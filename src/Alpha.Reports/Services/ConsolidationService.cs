namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Criteria.WinLossDetail;
    using Alpha.Reports.DataAccesses;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Helpers;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Params;
    using AlphaPlus.PartialUM.Services;
    using Constants;
    using Criteria.Consolidation;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Models.Consolidation;
    using Params.Consolidation;

    public interface ConsolidationService
    {
        IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param);

        IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate, string site);

        IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param);

        IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param);

        IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param);

        ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate, string site);

        PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);

        IEnumerable<int> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int custId, DateTime minReportDate, string site);
    }

    public class ConsolidationServiceImpl : ConsolidationService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly CacheRefService cacheRefService;
        private readonly WinLossDetailDataAccess winLossDetailDataAccess;

        private readonly Func<int, string> getProductName;
        private readonly IPartialUMService partialUMService;
        private readonly ICacheService cacheService;
        
        private const int CustProductsCacheTimeInMinutes = 2;

        public ConsolidationServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService,
            WinLossDetailDataAccess winLossDetailDataAccess,
            ProductService productService,
            Func<int, string> getProductName,
            IPartialUMService partialUMService,
            ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
            this.winLossDetailDataAccess = winLossDetailDataAccess;
            this.productService = productService;
            this.getProductName = getProductName;
            this.partialUMService = partialUMService;
            this.cacheService = cacheService;
        }

        public IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param)
        {
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new GetCurrencyCriteria(param.CustId, subfix);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var data = dynamicRepository.Fetch<CurrencyModel>(criteria);

            return data;
        }

        public IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param)
            => cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ConsolidationProductsForCustomer-{param.CustId}",
                () =>
                {
                    var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
                    var products = dynamicRepository
                        .Fetch<ConsolidationProduct>(new GetConsolidationReportProductsCriteria(param.CustId, subfix))
                        .Where(product => !IsExcludedProduct(product) && IsRemainSupportProduct(product, param.MinDate))
                        .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, cacheRefService.GetProductName));

                    return products;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

        public IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param)
        {
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new GetConsolidationCriteria(
                param.CustId,
                param.Currency,
                param.FromDate,
                param.ToDate,
                ConsolidationReportTypeConstants.Product,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.ProductIds.JoinWithComma(),
                param.ProductDetailsXML,
                subfix);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var rawdata = dynamicRepository.Fetch<ConsolidationByUsername>(criteria);

            return rawdata.Any()
                ? GetConsolidationByUsername(rawdata)
                : Enumerable.Empty<ConsolidationByUsername>();
        }

        public IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param)
        {
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new GetConsolidationCriteria(
                param.CustId,
                param.Currency,
                param.FromDate,
                param.ToDate,
                ConsolidationReportTypeConstants.Daily,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.ProductIds.JoinWithComma(),
                param.ProductDetailsXML,
                subfix);

            var data = dynamicRepository.Fetch<ConsolidationByDate>(criteria);

            return data;
        }

        public IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param)
        {
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);
            var criteria = new GetConsolidationCriteria(
                param.CustId,
                param.Currency,
                param.FromDate,
                param.ToDate,
                ConsolidationReportTypeConstants.Product,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.ProductIds.JoinWithComma(),
                param.ProductDetailsXML,
                subfix);

            var data = dynamicRepository.Fetch<ConsolidationByProduct>(criteria);

            return data;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate, string site)
        {
            var products = GetProducts(new GetConslidationReportProductsParam(custId, minReportDate, site))
                  .Select(product => product.Id)
                  .Where(id => productIds == null || !productIds.Any() || productIds.Contains(id));

            return products;
        }

        private static bool IsRemainSupportProduct(ConsolidationProduct product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        public ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate, string site)
        {
            var products = GetProducts(new GetConslidationReportProductsParam(custId, minReportDate, site)).ToList();

            return new ConsolidationExcludeProduct(products);
        }

        public PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            return winLossDetailDataAccess.GetData(GetWinLossDetailCriteria(param));
        }

        private WinLossDetailMemberCriteria GetWinLossDetailCriteria(WinLossDetailParam param)
        {
            var fromDate = param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo);
            var reportDate = reportDateService.GetBalanceUpDayAndMoveBetsDay();
            var validUserSelectedProductIds = param.UserSelectedProductIds.JoinWithComma();
            var subfix = FanexDataSettingKeyHelper.GetWinLossDetailSettingKeySubfix(param.Site);

            if (param.ReportLevel.IsAgentLevel())
            {
                return new WinLossDetailMemberCriteria(
                    param.CustId,
                    param.PageIndex,
                    param.PageSize,
                    fromDate,
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
                    fromDate,
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
                fromDate,
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

        public IEnumerable<int> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int custId, DateTime minReportDate, string site)
        {
            var data = productService.GetProductsNeedDelayTimeToSumarizeData();

            return FilterProductIdsByCustomer(custId, data.Select(i => (int)i.ProductId), minReportDate, site);
        }

        private IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(IEnumerable<ConsolidationByUsername> rawdata)
        {
            var memberValue = UserLevel.Member.Value;
            var smaCustIds = rawdata
                .Where(c => c.CustLevel != memberValue)
                .Select(c => c.CustId)
                .Distinct();
            var ptList = GetSMAPositionTakingList(smaCustIds);

            return from consolidation in rawdata
                   join pt in ptList
                      on new { consolidation.CustId, ProductId = consolidation.BreakDown }
                      equals new { pt.CustId, pt.ProductId } into joinData
                   from joinRecord in joinData.DefaultIfEmpty()
                   orderby consolidation.Username
                   select new ConsolidationByUsername(
                       consolidation.BreakDown,
                       consolidation.CustId,
                       consolidation.Username,
                       consolidation.CurrencyId,
                       consolidation.CustLevel,
                       consolidation.Turnover,
                       consolidation.GrossComm,
                       consolidation.MemberWinLoss,
                       consolidation.MemberComm,
                       consolidation.MemberTotal,
                       consolidation.OwnerTurnover,
                       consolidation.OwnerWinLoss,
                       consolidation.OwnerComm,
                       consolidation.OwnerTotal,
                       consolidation.Company,
                       consolidation.BetCount,
                       cacheRefService.GetProductName(consolidation.BreakDown),
                       joinRecord == null ? 0 : joinRecord.ValuePT);
        }

        private IEnumerable<SMAPositionTaking> GetSMAPositionTakingList(IEnumerable<int> smaCustIds)
            => dynamicRepository.Fetch<SMAPositionTaking>(new GetSMAPositionTakingCriteria(smaCustIds.JoinWithComma()));

        private static bool IsExcludedProduct(ConsolidationProduct product)
            => product.Id == ProductIdConstants.Cashout;
    }
}