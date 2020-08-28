namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Alpha.Reports.Constants;
    using Extensions;
    using Params;
    using Params.Consolidation;
    using Reports.Models;
    using Reports.Models.Consolidation;
    using Requests.Consolidation;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.Consolidation;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class ConsolidationBaseHandler : BaseHandler
    {
        protected readonly ConsolidationService reportService;
        protected readonly CacheRefService cacheRefService;
        protected readonly WinLossDetailService winLossDetailService;
        protected readonly IUser viewer;
        protected readonly ProductDetailService productDetailService;

        public ConsolidationBaseHandler(
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            WinLossDetailService winLossDetailService,
            IUser viewer,
            ProductDetailService productDetailService)
        {
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
            this.winLossDetailService = winLossDetailService;
            this.viewer = viewer;
            this.productDetailService = productDetailService;
        }

        protected GetConsolidationParam CreateParameter(GetConsolidationQuery message, IEnumerable<Product> products)
        {
            var validUserSelectedProductIds = reportService.FilterProductIdsByCustomer(
                message.CustId,
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomerForConsolidationReport(
                message.CustId,
                !message.IsPostBack ? DefaultVariables.AllProductDetails : message.UserSelectedResourceIds,
                products);

            return new GetConsolidationParam(
                 message.CustId,
                 message.UserSelectedCurrencyId,
                 message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                 message.ToDate,
                 validUserSelectedProductIds,
                 validUserSelectedResourceIds,
                 productDetailService.BuildProductDetailsXML(message.CustId, validUserSelectedProductIds, validUserSelectedResourceIds));
        }

        protected IEnumerable<ConsolidationByUsernameViewModel> BuildWinLossByUsernameViewModel(
            IEnumerable<ConsolidationByUsername> models,
            IDictionary<byte, string> currencies)
            => models.Select(model =>
                new ConsolidationByUsernameViewModel(
                    model.CustId,
                    model.CustLevel,
                    model.Username,
                    model.CurrencyId,
                    model.Turnover,
                    model.GrossComm,
                    model.MemberWinLoss,
                    model.MemberComm,
                    model.MemberTotal,
                    model.OwnerTurnover,
                    model.OwnerWinLoss,
                    model.OwnerComm,
                    model.OwnerTotal,
                    model.Company,
                    model.BetCount,
                    currencies[model.CurrencyId]))
                .ToList();

        protected IEnumerable<ConsolidationByDateViewModel> BuildWinLossByDateViewModel(
            IEnumerable<ConsolidationByDate> models,
            IDictionary<byte, string> currencies)
        {
            var viewModels = new List<ConsolidationByDateViewModel>();

            var dateGroups = models.GroupBy(model => model.Date);
            foreach (var dateGroup in dateGroups)
            {
                var winLossByUsername = BuildWinLossByUsernameViewModel(dateGroup, currencies);
                viewModels.Add(new ConsolidationByDateViewModel(dateGroup.Key, winLossByUsername));
            }

            return viewModels;
        }

        protected IEnumerable<ConsolidationByProductViewModel> BuildWinLossByProductViewModel(
            IEnumerable<ConsolidationByProduct> models,
            IDictionary<byte, string> currencies)
        {
            var viewModels = new List<ConsolidationByProductViewModel>();

            var productGroups = models.GroupBy(model => model.Product);
            foreach (var productGroup in productGroups)
            {
                var winLossByUsername = BuildWinLossByUsernameViewModel(productGroup, currencies);
                viewModels.Add(new ConsolidationByProductViewModel(productGroup.Key, cacheRefService.GetProductName(productGroup.Key), winLossByUsername));
            }

            return viewModels;
        }

        protected WinLossDetailQuery CreateWinLossDetailQuery(ConsolidationDetailQuery message)
            => new WinLossDetailQuery(viewer)
            {
                UserSelectedProductIds = message.UserSelectedProductIds,
                UserSelectedResourceIds = message.UserSelectedResourceIds,
                SortPropertyName = message.SortPropertyName,
                IsDescendingSort = message.IsDescendingSort,
                IsStackMode = message.IsStackMode,
                IsFilterVisible = message.IsFilterVisible,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                FromDate = message.FromDate,
                ToDate = message.ToDate,

                CustId = message.CustId,
                CustName = message.CustName,
                CustLevelId = message.CustLevelId,
                BreadcrumbLevel = message.BreadcrumbLevel
            };

        protected WinLossDetailParam CreateWinLossDetailParam(WinLossDetailQuery query, IEnumerable<Product> products)
        {
            var validUserSelectedProductIds =
               reportService.FilterProductIdsByCustomer(query.ViewerId, query.Site, query.CurrencyId, query.UserSelectedProductIds.ToListInt(), query.MinDate);
            var validUserSelectedResourceIds =
                productDetailService.FilterResourceIdsByCustomerForConsolidationReport(query.ViewerId, query.UserSelectedResourceIds, products);

            return new WinLossDetailParam(
                query.CustId,
                query.Site,
                query.CurrencyId,
                query.PageIndex,
                query.PageSize,
                query.FromDate,
                query.ToDate,
                query.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                validUserSelectedResourceIds,
                query.CustLevel,
                query.SortPropertyName,
                query.IsDescendingSort,
                query.IsHistoricReport,
                productDetailService.BuildProductDetailsXML(query.ViewerId, validUserSelectedProductIds, validUserSelectedResourceIds));
        }
    }
}