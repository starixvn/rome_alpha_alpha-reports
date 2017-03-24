namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Requests.MatchWinLossDetail;
    using ReportHelpers = Helpers;

    public class MatchWinLossDetailReportViewModel
    {
        private const string DefaultBetListPath = "~/_BetList/BetList.aspx";
        private readonly MatchWinLossDetailQuery query;
        private readonly MatchWinLossDetailParam param;

        public MatchWinLossDetailReportViewModel(
            IEnumerable<MatchWinLossDetailViewModel> noParlayMatchWinLossDetail,
            BetTypeWinLossViewModel noParlayWinLossTotal,
            BetTypeWinLossViewModel mixParlay,
            BetTypeWinLossViewModel systemParlay,
            BetTypeWinLossViewModel grandTotal,
            MatchWinLossDetailQuery query,
            MatchWinLossDetailParam param,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> leagues)
        {
            NoParlayMatchWinLossDetail = noParlayMatchWinLossDetail;
            NoParlayWinLossTotal = noParlayWinLossTotal;

            MixParlay = mixParlay;
            SystemParlay = systemParlay;
            GrandTotal = grandTotal;

            this.query = query;
            this.param = param;

            Products = products;
            Leagues = leagues;
        }

        public string Title => Resources.MatchWinLossDetail;

        public int CustId => param.CustId;

        public DateTime MinDate => query.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public string Language => query.Language;

        public string UserSelectedProductIds => param.UserSelectedProductIds.JoinWithComma();

        public string UserSelectedLeagueIds => param.UserSelectedLeagueIds.JoinWithComma();

        public int UserSelectedDateRangeOption => query.UserSelectedDateRangeOption;

        public bool IsStackMode => query.IsStackMode;

        public bool IsFilterVisible => query.IsFilterVisible;

        public bool HasData =>
            (NoParlayMatchWinLossDetail != null && NoParlayMatchWinLossDetail.Any())
            || MixParlay != null
            || SystemParlay != null;

        public int ReportColumnsCount
            => CustLevel.IsSuperLevel()
            ? 17
            : CustLevel.IsMasterLevel()
            ? 14
            : 11;

        public bool IsSetDefaultStackMode => query.IsSetDefaultStackMode;

        public bool ShowTotalRow => NoParlayWinLossTotal != null && (MixParlay != null || SystemParlay != null);

        public UserLevel CustLevel => query.CustLevel;

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> Leagues { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        private LeagueMultiSelectionViewModel LeagueMultiSelectionViewModel =>
            new LeagueMultiSelectionViewModel(Leagues, UserSelectedLeagueIds, Language, query.IsUserSelectsAllLeagues);

        public MatchWinLossDetailFilterViewModel FilterViewModel =>
            new MatchWinLossDetailFilterViewModel(
                CustId,
                IsFilterVisible,
                CustLevel,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel,
                LeagueMultiSelectionViewModel,
                MinDate);

        public IEnumerable<MatchWinLossDetailViewModel> NoParlayMatchWinLossDetail { get; }

        public BetTypeWinLossViewModel NoParlayWinLossTotal { get; }

        public BetTypeWinLossViewModel MixParlay { get; }

        public BetTypeWinLossViewModel SystemParlay { get; }

        public BetTypeWinLossViewModel GrandTotal { get; }

        public string GetBetListLink(IDictionary<string, object> routeValues)
            => ReportHelpers.UrlHelper.AddQueryStringParamsFunc(ReportHelpers.UrlHelper.MapExMainPath(DefaultBetListPath), routeValues);

        public IDictionary<string, object> GetBetListLinkRouteValues(
            MatchWinLossDetailViewModel match,
            BetTypeWinLossViewModel betType,
            DateTime fromDate,
            DateTime toDate)
        {
            var result = new Dictionary<string, object>
            {
                ["BetType"] = betType.Id,
                ["Type"] = "WLByBettypeMatch",
                ["IsLive"] = betType.IsLive ? 1 : 0
            };

            if (match.Race > 0)
            {
                result["leagueid"] = match.LeagueId;
                result["fdate"] = fromDate;
                result["tdate"] = toDate;
                result["race"] = betType.Race;
            }
            else
            {
                result["matchid"] = match.MatchId;

                if (betType.Id == BetTypeIdConstants.Outright)
                {
                    result["leagueid"] = match.LeagueId;
                    result["fdate"] = fromDate;
                    result["tdate"] = toDate;
                }
            }

            return result;
        }

        public IDictionary<string, object> GetParlayBetListLinkRouteValues(int betType, DateTime fromDate, DateTime toDate)
        {
            return new Dictionary<string, object>
            {
                ["fdate"] = fromDate,
                ["tdate"] = toDate,
                ["bettype"] = betType,
                ["type"] = "MixParlayByMatch"
            };
        }
    }
}