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
        private readonly MatchWinLossDetailQuery query;
        private readonly MatchWinLossDetailParam param;

        public MatchWinLossDetailReportViewModel(
            IEnumerable<MatchWinLossDetailViewModel> noParlayMatchWinLossDetail,
            BetTypeWinLossViewModel noParlayWinLossTotal,
            BetTypeWinLossViewModel mixParlay,
            BetTypeWinLossViewModel systemParlay,
            IEnumerable<BetTypeWinLossViewModel> parlayMatchWinLossDetail,
            BetTypeWinLossViewModel grandTotal,
            MatchWinLossDetailQuery query,
            MatchWinLossDetailParam param,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<SelectListItem> leagues)
        {
            NoParlayMatchWinLossDetail = noParlayMatchWinLossDetail;
            NoParlayWinLossTotal = noParlayWinLossTotal;

            MixParlay = mixParlay;
            SystemParlay = systemParlay;
            ParlayMatchWinLossDetail = parlayMatchWinLossDetail;
            GrandTotal = grandTotal;

            this.query = query;
            this.param = param;

            GroupProducts = groupProducts;
            Leagues = leagues;

            BetListPath = Helpers.UrlHelper.MapBetListPath("~/WinLossByMatch/Index");
        }

        public string Title => Resources.MatchWinLossDetail;

        public int CustId => param.CustId;

        public string CustName => query.CustName;

        public int CustLevelId => query.CustLevelId;

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
            || SystemParlay != null
            || (ParlayMatchWinLossDetail != null && ParlayMatchWinLossDetail.Any());

        public int ReportColumnsCount
            => CustLevel.IsSuperLevel()
            ? 17
            : CustLevel.IsMasterLevel()
            ? 14
            : 11;

        public bool IsSetDefaultStackMode => query.IsSetDefaultStackMode;

        public bool ShowTotalRow => NoParlayWinLossTotal != null && (MixParlay != null || SystemParlay != null || (ParlayMatchWinLossDetail != null && ParlayMatchWinLossDetail.Any()));

        public UserLevel CustLevel => query.CustLevel;

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public IEnumerable<SelectListItem> Leagues { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(GroupProducts, UserSelectedProductIds, Language);

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

        public IEnumerable<BetTypeWinLossViewModel> ParlayMatchWinLossDetail { get; }

        public BetTypeWinLossViewModel GrandTotal { get; }

        public string BetListPath { get; }

        public string GetBetListLink(IDictionary<string, object> routeValues)
            => ReportHelpers.UrlHelper.AddQueryStringParamsFunc(BetListPath, routeValues);

        public IDictionary<string, object> GetBetListLinkRouteValues(
            MatchWinLossDetailViewModel match,
            BetTypeWinLossViewModel betType,
            DateTime fromDate,
            DateTime toDate)
        {
            var result = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["CustLevelId"] = CustLevelId,
                ["BetType"] = betType.Id,
                ["LiveIndicator"] = betType.IsLive ? 1 : 0,
                ["LeagueId"] = match.LeagueId,
                ["FromDate"] = fromDate,
                ["ToDate"] = toDate
        };

            if (match.Race > 0)
            {   
                result["Race"] = betType.Race;
            }
            else
            {
                result["MatchId"] = match.MatchId;                
            }

            return result;
        }

        public string GetParlayBetListLink(int betType, DateTime fromDate, DateTime toDate)
{
    var routeValues = new Dictionary<string, object>
    {
        ["CustId"] = CustId,
        ["CustName"] = CustName,
        ["CustLevelId"] = CustLevelId,
        ["FromDate"] = fromDate,
        ["ToDate"] = toDate,
        ["BetType"] = betType
    };

    return ReportHelpers.UrlHelper.AddQueryStringParamsFunc(Helpers.UrlHelper.MapBetListPath("~/ParlayWinLossByMatch/Index"), routeValues);
}

public string GetBetListLinkByBetType(int bettype, DateTime fromDate, DateTime toDate)
{
    var routeValues = new Dictionary<string, object>
    {
        ["CustId"] = CustId,
        ["CustName"] = CustName,
        ["CustLevelId"] = CustLevelId,
        ["FromDate"] = fromDate,
        ["ToDate"] = toDate,
        ["BetType"] = bettype
    };

    return ReportHelpers.UrlHelper.AddQueryStringParamsFunc(Helpers.UrlHelper.MapBetListPath(BetListPath), routeValues);
}
    }
}