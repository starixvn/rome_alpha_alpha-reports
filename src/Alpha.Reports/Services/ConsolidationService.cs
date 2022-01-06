namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using Alpha.Reports.Criteria.WinLossDetail;
    using Alpha.Reports.DataAccesses;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Params;
    using Alpha.Web.Helpers;
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;
    using Constants;
    using Criteria.Consolidation;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using Models;
    using Models.Consolidation;
    using Params.Consolidation;

    public interface ConsolidationService
    {
        IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param);

        IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param, bool isEnableSabaPromotion);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate, bool isEnableSabaPromotion);

        IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param);

        IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param);

        IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param);

        ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate, bool isEnableSabaPromotion);

        PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);

        IEnumerable<int> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int custId, DateTime minReportDate, bool isEnableSabaPromotion);
    }

    public class ConsolidationServiceImpl : ConsolidationService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly CacheRefService cacheRefService;
        private readonly WinLossDetailDataAccess winLossDetailDataAccess;

        private readonly Func<int, string> getProductName;
        private readonly PartialUMService partialUMService;
        private readonly ICacheService cacheService;

        private const int SabaPromotionProductId = 37;
        private const int CustProductsCacheTimeInMinutes = 2;

        public ConsolidationServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService,
            WinLossDetailDataAccess winLossDetailDataAccess,
            ProductService productService,
            Func<int, string> getProductName,
            PartialUMService partialUMService,
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
            var criteria = new GetCurrencyCriteria(param.CustId);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var data = dynamicRepository.Fetch<CurrencyModel>(criteria);

            return data;
        }

        public IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param, bool isEnableSabaPromotion)
            => cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ConsolidationProductsForCustomer-{param.CustId}-{isEnableSabaPromotion}",
                () =>
                {
                    var products = dynamicRepository
                        .Fetch<ConsolidationProduct>(new GetConsolidationReportProductsCriteria(param.CustId))
                        .Where(product => !IsExcludedProduct(product) && IsRemainSupportProduct(product, param.MinDate))
                        .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, cacheRefService.GetProductName));

                    if (isEnableSabaPromotion)
                    {
                        var sabaProduct = new Product(SabaPromotionProductId, null, products.Max(product => product.EndOfSupportDate), 0, 0, getProductName);
                        var sabaProducts = new List<Product> { sabaProduct };
                        products = sabaProducts.Concat(products);
                    }

                    return products;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

        public IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Product,
                param.ProductDetailsXML);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var rawdata = dynamicRepository.Fetch<ConsolidationByUsername>(criteria);

            return rawdata.Any()
                ? GetConsolidationByUsername(rawdata)
                : Enumerable.Empty<ConsolidationByUsername>();
        }

        public IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Daily,
                param.ProductDetailsXML);

            var data = dynamicRepository.Fetch<ConsolidationByDate>(criteria);

            return data;
        }

        public IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Product,
                param.ProductDetailsXML);

            var data = dynamicRepository.Fetch<ConsolidationByProduct>(criteria);

            return data;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate, bool isEnableSabaPromotion)
        {
            var products = GetProducts(new GetConslidationReportProductsParam(custId, minReportDate), isEnableSabaPromotion)
                  .Select(product => product.Id)
                  .Where(id => productIds == null || !productIds.Any() || productIds.Contains(id));

            return products;
        }

        private static bool IsRemainSupportProduct(ConsolidationProduct product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        public ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate, bool isEnableSabaPromotion)
        {
            var products = GetProducts(new GetConslidationReportProductsParam(custId, minReportDate), isEnableSabaPromotion).ToList();

            return new ConsolidationExcludeProduct(products);
        }

        public PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.UserSelectedProductIds,
                param.ReportLevel,
                param.ProductDetailsXML);

            var winLossDetailData = winLossDetailDataAccess.GetData(criteria);

            return winLossDetailData;
        }

        public IEnumerable<int> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int custId, DateTime minReportDate, bool isEnableSabaPromotion)
        {
            var data = productService.GetProductsNeedDelayTimeToSumarizeData();

            return FilterProductIdsByCustomer(custId, data.Select(i => (int)i.ProductId), minReportDate, isEnableSabaPromotion);
        }

        private IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(IEnumerable<ConsolidationByUsername> rawdata)
        {
            var memberValue = UserLevel.Member.Value;
            var smaCustIds = rawdata
                .Where(c => c.CustLevel != memberValue)
                .Select(c => c.CustId.ToString())
                .Distinct()
                .ToArray();

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

        private IEnumerable<SMAPositionTaking> GetSMAPositionTakingList(string[] smaCustIds)
        {
            var ptList = dynamicRepository.Fetch<SMAPositionTaking>(
                new GetSMAPositionTakingCriteria(smaCustIds.JoinWithComma()));
            var casinoSMAPTList = GetCasinoSMAPositionTakingList(smaCustIds);

            return ptList.Concat(casinoSMAPTList);
        }

        private static IEnumerable<SMAPositionTaking> GetCasinoSMAPositionTakingList(string[] smaCustIds)
        {
            var customer = new Customer();
            var ptList = new List<SMAPositionTaking>();

            try
            {
                var ptTable = customer.GetCustDesktopPT(smaCustIds);

                foreach (DataRow row in ptTable.Rows)
                {
                    ptList.Add(new SMAPositionTaking(
                        Convert.ToInt32(row["OBSuperID"]),
                        Convert.ToDecimal(row["PositionTaking"]),
                        ProductIdConstants.RNGCasino));
                }
            }
            catch (ErrorException casinoEx)
            {
                ExceptionHandler.Handle(new Exception(casinoEx.Message, casinoEx), TraceEventType.Information);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return ptList;
        }

        private static bool IsExcludedProduct(ConsolidationProduct product)
            => product.Id == ProductIdConstants.Cashout;
    }
}
