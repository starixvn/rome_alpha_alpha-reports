namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Site.Features.EnteredResult;
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
    public class ConsolidationBaseHandler : EnteredSportResultBaseHandler
    {
        protected readonly ConsolidationService consolidationService;
        protected readonly CacheRefService cacheRefService;
        protected readonly IUser viewer;
        protected readonly ProductDetailService productDetailService;

        public ConsolidationBaseHandler(
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            IUser viewer,
            ProductDetailService productDetailService,
            WinLossService winLossService) : base(winLossService)
        {
            this.consolidationService = reportService;
            this.cacheRefService = cacheRefService;
            this.viewer = viewer;
            this.productDetailService = productDetailService;
        }

        protected GetConsolidationParam CreateParameter(GetConsolidationQuery message, IEnumerable<Product> products)
        {
            var validUserSelectedProductIds = consolidationService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate,
                message.Site);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomer(
                message.CustId,
                !message.IsPostBack ? DefaultVariables.AllProductDetails : message.UserSelectedResourceIds,
                products);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, validUserSelectedResourceIds);

            return new GetConsolidationParam(
                 message.CustId,
                 message.UserSelectedCurrencyId,
                 message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                 message.ToDate,
                 validUserSelectedProductIds,
                 validUserSelectedResourceIds,
                 productDetailService.BuildProductDetailsXML(validUserSelectedResourceIds),
                 message.Site);
        }

        protected IEnumerable<ConsolidationEntityViewModel> BuildWinLossByUsernameViewModel(
            IEnumerable<ConsolidationEntity> models,
            IDictionary<byte, string> currencies)
            => models.Select(model =>
                new ConsolidationEntityViewModel(
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
                LevelType = message.LevelType,
                CustId = message.CustId,
                CustName = message.CustName,
                CustLevelId = message.CustLevelId,
                BreadcrumbLevel = message.BreadcrumbLevel
            };

        protected WinLossDetailParam CreateWinLossDetailParam(WinLossDetailQuery query, IEnumerable<Product> products)
        {
            var validUserSelectedProductIds =
               consolidationService.FilterProductIdsByCustomer(query.ViewerId, query.UserSelectedProductIds.ToListInt(), query.MinDate, query.Site);
            var validUserSelectedResourceIds =
                productDetailService.FilterResourceIdsByCustomer(query.ViewerId, query.UserSelectedResourceIds, products);

            return new WinLossDetailParam(
                query.CustId,
                query.ViewerId,
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
                productDetailService.BuildProductDetailsXML(validUserSelectedResourceIds),
                username: null,
                levelType: query.LevelType,
                isReward: false);
        }

        protected IEnumerable<ConsolidationByUsernameViewModel> BuildConsolidationByUsernameViewModel(
            IEnumerable<ConsolidationByUsername> consolidationByUsernames,
            IDictionary<byte, string> currencies)
            => consolidationByUsernames.GroupBy(
                consolidation => consolidation.CustId,
                (custIdKey, consolidationByCustIds) =>
                {
                    var total = new ConsolidationByUsernameViewModel();
                    var consolidationByCustIdList = consolidationByCustIds.OrderByDescending(c => c.PositionTaking).ToList();

                    consolidationByCustIdList.ForEach(item => total = AccumulateTotal(total, item, currencies));
                    total.Children = BuildConsolidationByUsernameViewModelGroupByPT(consolidationByCustIdList);

                    return total;
                });

        private static IEnumerable<ConsolidationByUsernameViewModel> BuildConsolidationByUsernameViewModelGroupByPT(
            IEnumerable<ConsolidationByUsername> consolidationByUsernames)
            => consolidationByUsernames.GroupBy(
                consolidation => consolidation.PositionTaking,
                (ptKey, consolidationByPTs) =>
                {
                    var total = new ConsolidationByUsernameViewModel();
                    var consolidationByPTList = consolidationByPTs.OrderByDescending(c => c.Turnover).ToList();

                    consolidationByPTList.ForEach(item => total = AccumulateTotal(total, item));
                    total.Children = consolidationByPTList.Select(c => new ConsolidationByUsernameViewModel(
                        c.CustId,
                        c.CustLevel,
                        c.Username,
                        c.CurrencyId,
                        c.Turnover,
                        c.GrossComm,
                        c.MemberWinLoss,
                        c.MemberComm,
                        c.MemberTotal,
                        c.OwnerTurnover,
                        c.OwnerWinLoss,
                        c.OwnerComm,
                        c.OwnerTotal,
                        c.Company,
                        c.BetCount,
                        productName: c.ProductName));

                    return total;
                });

        private static ConsolidationByUsernameViewModel AccumulateTotal(
            ConsolidationByUsernameViewModel source,
            ConsolidationByUsername destination,
            IDictionary<byte, string> currencies = null)
            => new ConsolidationByUsernameViewModel(
                destination.CustId,
                destination.CustLevel,
                destination.Username,
                destination.CurrencyId,
                source.Turnover + destination.Turnover,
                source.GrossComm + destination.GrossComm,
                source.MemberWinLoss + destination.MemberWinLoss,
                source.MemberComm + destination.MemberComm,
                source.MemberTotal + destination.MemberTotal,
                source.OwnerTurnover + destination.OwnerTurnover,
                source.OwnerWinLoss + destination.OwnerWinLoss,
                source.OwnerComm + destination.OwnerComm,
                source.OwnerTotal + destination.OwnerTotal,
                source.Company + destination.Company,
                source.BetCount + destination.BetCount,
                currencies == null ? null : currencies[destination.CurrencyId],
                positionTaking: destination.PositionTaking);
    }
}