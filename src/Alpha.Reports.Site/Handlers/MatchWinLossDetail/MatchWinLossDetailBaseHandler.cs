namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using Fanex.Common;
    using Params;
    using RemotingRef;
    using Reports.Constants;
    using Reports.Helpers;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Requests.MatchWinLossDetail;
    using Services;
    using ViewModels.MatchWinLossDetail;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class MatchWinLossDetailBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly LeagueService leagueService;
        protected readonly CacheRefService cacheRefService;
        protected readonly MatchWinLossDetailService reportService;

        public MatchWinLossDetailBaseHandler(
            ProductService productService,
            LeagueService leagueService,
            MatchWinLossDetailService reportService,
            CacheRefService cacheRefService)
        {
            this.productService = productService;
            this.leagueService = leagueService;
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected MatchWinLossDetailParam CreateParameter(MatchWinLossDetailQuery query, IEnumerable<Product> supportedProducts)
        {
            var productIds = supportedProducts.Select(product => product.Id);
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                query.CustId,
                query.UserSelectedProductIds.ToListInt())
                .Where(product => productIds.Contains(product));

            return new MatchWinLossDetailParam(
                 query.CustId,
                 query.FromDate,
                 query.ToDate,
                  query.UserSelectedLeagueIds.ToListInt(),
                 validUserSelectedProductIds,
                 query.CustLevel);
        }

        protected BetTypeWinLossViewModel GetMixParlay(IEnumerable<MatchWinLossDetailModel> reportData)
        {
            var mixParlayData = reportData.FirstOrDefault(data => data.IsParlay && data.BetType == BetTypeIdConstants.MixParlay);
            return mixParlayData == null
                ? null
                : new BetTypeWinLossViewModel(mixParlayData, cacheRefService.GetBetType(mixParlayData.BetType));
        }

        protected BetTypeWinLossViewModel GetSystemParlay(IEnumerable<MatchWinLossDetailModel> reportData)
        {
            var systemParlayData = reportData.FirstOrDefault(data => data.IsParlay && data.BetType == BetTypeIdConstants.SystemParlay);
            return systemParlayData == null
                ? null
                : new BetTypeWinLossViewModel(systemParlayData, cacheRefService.GetBetType(systemParlayData.BetType));
        }

        protected IEnumerable<MatchWinLossDetailViewModel> GetNoParlayMatchWinLossDetail(IEnumerable<MatchWinLossDetailModel> reportData)
        {
            var noParlayData = reportData.Where(data => !data.IsParlay);
            var leagueNames = leagueService.GetLeagueNames(noParlayData.Select(data => data.LeagueId));
            var teamIds = noParlayData.Select(data => data.HomeId).Concat(noParlayData.Select(data => data.AwayId));
            var teamNames = GetTeamNames(teamIds);
            var betTypeNames = GetBetTypeNames(noParlayData.Select(data => data.BetType));
            var matchGroups = noParlayData.GroupBy(data => GetGroupId(data, leagueNames[data.LeagueId]));

            var noParlayMatchWinLossDetail = new List<MatchWinLossDetailViewModel>();

            foreach (var matchGroup in matchGroups)
            {
                noParlayMatchWinLossDetail.Add(BuildMatchWinLossDetailViewModel(matchGroup, leagueNames, teamNames, betTypeNames));
            }

            return noParlayMatchWinLossDetail;
        }

        internal Dictionary<int, string> GetTeamNames(IEnumerable<int> teamIds)
        {
            var teamNames = new Dictionary<int, string>();

            if (teamIds == null)
            {
                return teamNames;
            }

            var distinctTeamIds = teamIds.Distinct();

            foreach (var teamId in distinctTeamIds)
            {
                teamNames.Add(teamId, cacheRefService.GetTeamName(teamId));
            }

            return teamNames;
        }

        internal Dictionary<int, string> GetBetTypeNames(IEnumerable<int> betTypeIds)
        {
            var betTypeNames = new Dictionary<int, string>();

            if (betTypeIds == null)
            {
                return betTypeNames;
            }

            var distinctBetTypeIds = betTypeIds.Distinct();

            foreach (var betTypeId in distinctBetTypeIds)
            {
                betTypeNames.Add(betTypeId, cacheRefService.GetBetType(betTypeId));
            }

            return betTypeNames;
        }

        internal string GetGroupId(MatchWinLossDetailModel data, string leagueName)
        {
            if (data.BetType == BetTypeIdConstants.Outright)
            {
                return $"{leagueName}_Match{data.MatchId}";
            }

            if (BetTypeHelper.IsRacing(data.BetType))
            {
                return $"{leagueName}_Race{data.Race}";
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return $"{leagueName}_MatchCode{data.MatchCode}";
            }

            if (BetTypeHelper.IsVirtualHorseRacing(data.BetType))
            {
                DateTime eventTime;
                bool isValidShowTime = DateTime.TryParse(data.ShowTime, out eventTime);

                return isValidShowTime ? $"{leagueName}_ShowTime{data.ShowTime}" : leagueName;
            }

            return $"{leagueName}_MatchId{data.MatchId}";
        }

        internal MatchWinLossDetailViewModel BuildMatchWinLossDetailViewModel(
            IGrouping<string, MatchWinLossDetailModel> matchGroup,
            Dictionary<int, string> leagueNames,
            Dictionary<int, string> teamNames,
            Dictionary<int, string> betTypeNames)
        {
            var betTypeList = matchGroup.ToList();
            var firstWinLoss = betTypeList[0];
            var leagueName = leagueNames[firstWinLoss.LeagueId];
            var betTypeWinLoss = new List<BetTypeWinLossViewModel>();

            var total = new BetTypeWinLossViewModel();

            foreach (var winLoss in matchGroup)
            {
                var betTypeWinLossViewModel = new BetTypeWinLossViewModel(
                    winLoss.BetType,
                    GetBetTypePrefix(winLoss) + betTypeNames[winLoss.BetType],
                    winLoss.Turnover,
                    winLoss.GrossComm,
                    winLoss.LiveIndicator,
                    winLoss.Race,
                    winLoss.MemberNetWinLoss,
                    winLoss.AgentNetWinLoss,
                    winLoss.MasterNetWinLoss,
                    winLoss.SuperNetWinLoss,
                    winLoss.CompanyNetWinLoss);

                betTypeWinLoss.Add(betTypeWinLossViewModel);
                total.Add(betTypeWinLossViewModel);
            }

            return new MatchWinLossDetailViewModel(
                firstWinLoss.LeagueId,
                GetLeagueTitle(firstWinLoss, leagueName),
                firstWinLoss.MatchId,
                GetMatchTitle(firstWinLoss, teamNames),
                firstWinLoss.Race,
                betTypeWinLoss,
                total);
        }

        internal string GetMatchTitle(MatchWinLossDetailModel data, Dictionary<int, string> teamNames)
        {
            if (data.BetType == BetTypeIdConstants.Outright)
            {
                return string.Empty;
            }

            if (BetTypeHelper.IsRacing(data.BetType))
            {
                return $"{Resources.Race} {data.Race}";
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return $"{Resources.NumberGameNo} {data.MatchCode}";
            }

            if (BetTypeHelper.IsVirtualHorseRacing(data.BetType))
            {
                DateTime eventTime;
                bool isValidShowTime = DateTime.TryParse(data.ShowTime, out eventTime);

                return isValidShowTime ? eventTime.ToLongDateWithSlash() : string.Empty;
            }

            return string.Join(
                string.Empty,
                teamNames[data.HomeId],
                data.IsNeutral ? Resources.Neutral : string.Empty,
                Resources.vs,
                teamNames[data.AwayId]);
        }

        internal string GetLeagueTitle(MatchWinLossDetailModel data, string leagueName)
        {
            if (BetTypeHelper.IsRacing(data.BetType))
            {
                return $"{Resources.HorseRacing} - {leagueName}";
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return string.IsNullOrWhiteSpace(leagueName)
                    ? Resources.NumberGame
                    : $"{Resources.NumberGame} - {leagueName}";
            }

            return leagueName;
        }

        internal string GetBetTypePrefix(MatchWinLossDetailModel winLoss)
        {
            switch (winLoss.BetType)
            {
                case BetTypeIdConstants.NumberGameOverUnder:
                    return $"{Resources.Next} ";

                case BetTypeIdConstants.NumberGameOddEven:
                    if (winLoss.LiveIndicator)
                    {
                        return $"{Resources.Next} ";
                    }

                    return $"{Resources.FT} ";

                default:
                    return string.Empty;
            }
        }

        internal BetTypeWinLossViewModel BuildWinLossTotal(IEnumerable<MatchWinLossDetailViewModel> matchWinLossDetail)
        {
            var total = new BetTypeWinLossViewModel();

            foreach (var winLoss in matchWinLossDetail)
            {
                total.Add(winLoss.Total);
            }

            return total;
        }

        internal BetTypeWinLossViewModel BuildWinLossGrandTotal(
            BetTypeWinLossViewModel noParlayWinLossTotal,
            BetTypeWinLossViewModel mixParlay,
            BetTypeWinLossViewModel systemParlay)
        {
            var grandTotal = new BetTypeWinLossViewModel()
                .Add(noParlayWinLossTotal)
                .Add(mixParlay)
                .Add(systemParlay);

            return grandTotal;
        }
    }
}