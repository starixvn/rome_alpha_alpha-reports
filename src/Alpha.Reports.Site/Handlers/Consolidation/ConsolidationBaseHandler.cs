namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Extensions;
    using Params;
    using Params.Consolidation;
    using RemotingRef;
    using Reports.Models;
    using Reports.Models.Consolidation;
    using Requests.Consolidation;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.Consolidation;
    using static global::RemotingRef.RefData;

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

        private Func<int, ExchangeData> getCurrencyName;

        public ConsolidationBaseHandler(
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            WinLossDetailService winLossDetailService,
            IUser viewer)
        {
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
            this.winLossDetailService = winLossDetailService;
            this.viewer = viewer;
        }

        public Func<int, ExchangeData> GetCurrencyName
        {
            get { return getCurrencyName ?? GetExchangeData; }
            set { getCurrencyName = value; }
        }

        protected GetConsolidationParam CreateParameter(GetConsolidationQuery message)
        {
            var validUserSelectedProductIds = reportService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);

            return new GetConsolidationParam(
                 message.CustId,
                 message.UserSelectedCurrencyId,
                 message.FromDate.GetLaterDate(message.LimitedDateToShowInfo),
                 message.ToDate,
                 validUserSelectedProductIds);
        }

        protected IEnumerable<ConsolidationByUsernameViewModel> BuildWinLossByUsernameViewModel(IEnumerable<ConsolidationByUsername> models)
        {
            var viewModels = new List<ConsolidationByUsernameViewModel>();

            foreach (var model in models)
            {
                viewModels.Add(new ConsolidationByUsernameViewModel(
                    model.CustId,
                    model.CustLevel,
                    model.Username,
                    model.CurrencyId,
                    model.Turnover,
                    model.MemberWinLoss,
                    model.MemberComm,
                    model.MemberTotal,
                    model.OwnerTurnover,
                    model.OwnerWinLoss,
                    model.OwnerComm,
                    model.OwnerTotal,
                    model.Company,
                    model.BetCount,
                    GetCurrencyName(model.CurrencyId).Currency));
            }

            return viewModels;
        }

        protected IEnumerable<ConsolidationByDateViewModel> BuildWinLossByDateViewModel(IEnumerable<ConsolidationByDate> models)
        {
            var viewModels = new List<ConsolidationByDateViewModel>();

            var dateGroups = models.GroupBy(model => model.Date);
            foreach (var dateGroup in dateGroups)
            {
                var winLossByUsername = BuildWinLossByUsernameViewModel(dateGroup);
                viewModels.Add(new ConsolidationByDateViewModel(dateGroup.Key, winLossByUsername));
            }

            return viewModels;
        }

        protected IEnumerable<ConsolidationByProductViewModel> BuildWinLossByProductViewModel(IEnumerable<ConsolidationByProduct> models)
        {
            var viewModels = new List<ConsolidationByProductViewModel>();

            var productGroups = models.GroupBy(model => model.Product);
            foreach (var productGroup in productGroups)
            {
                var winLossByUsername = BuildWinLossByUsernameViewModel(productGroup);
                viewModels.Add(new ConsolidationByProductViewModel(productGroup.Key, cacheRefService.GetProductName(productGroup.Key), winLossByUsername));
            }

            return viewModels;
        }

        protected WinLossDetailQuery CreateWinLossDetailQuery(ConsolidationDetailQuery message)
            => new WinLossDetailQuery(viewer)
            {
                UserSelectedProductIds = message.UserSelectedProductIds,
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

        protected WinLossDetailParam CreateWinLossDetailParam(WinLossDetailQuery query)
        {
            var validUserSelectedProductIds =
               reportService.FilterProductIdsByCustomer(query.ViewerId, query.UserSelectedProductIds.ToListInt(), query.MinDate);

            return new WinLossDetailParam(
                query.CustId,
                query.PageIndex,
                query.PageSize,
                query.FromDate,
                query.ToDate,
                query.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                query.CustLevel,
                query.SortPropertyName,
                query.IsDescendingSort,
                query.IsHistoricReport);
        }
    }
}