namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Extensions;
    using Params;
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
            var validUserSelectedProductIds = productService.FilterProductIds(
                query.Site,
                query.CurrencyId,
                query.UserSelectedProductIds.ToListInt())
                .Where(product => productIds.Contains(product));

            return new MatchWinLossDetailParam(
                 query.CustId,
                 query.ViewerId,
                 query.Site,
                 query.CurrencyId,
                 query.FromDate,
                 query.ToDate,
                 query.LimitedDateToShowInfo,
                  query.UserSelectedLeagueIds.ToListString(),
                 validUserSelectedProductIds,
                 query.CustLevel);
        }

        protected BetTypeWinLossViewModel GetMixParlay(IEnumerable<MatchWinLossDetailModel> reportData)
        {
            var mixParlayData = reportData.FirstOrDefault(data => data.IsParlay &&
                (data.BetType == BetTypeIdConstants.MixParlay || data.BetType == BetTypeIdConstants.MixParlayBetRadarVirtualSports));

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

        public IEnumerable<BetTypeWinLossViewModel> GetParlayMatchWinLossDetail(IEnumerable<MatchWinLossDetailModel> reportData)
        {
            var data = reportData.Where(i => i.IsParlay && IsParlayMatchWinLossDetail(i.BetType));

            return data.Select(i => new BetTypeWinLossViewModel(i, cacheRefService.GetBetType(i.BetType)));
        }
        
        protected IEnumerable<MatchWinLossDetailViewModel> GetNoParlayMatchWinLossDetail(IEnumerable<MatchWinLossDetailModel> reportData)
        {
            var noParlayData = reportData.Where(data => !data.IsParlay);
            var betTypeNames = GetBetTypeNames(noParlayData.Select(data => data.BetType));
            var matchGroups = noParlayData.GroupBy(data => GetGroupId(data, leagueService.GetLeagueName(data.LeagueId, data.LeagueType, data.LeagueGroupId)));

            var noParlayMatchWinLossDetail = new List<MatchWinLossDetailViewModel>();

            foreach (var matchGroup in matchGroups)
            {
                noParlayMatchWinLossDetail.Add(BuildMatchWinLossDetailViewModel(matchGroup, betTypeNames));
            }

            return noParlayMatchWinLossDetail;
        }

        internal string GetTeamName(int teamId, byte leagueType)
            => leagueType == (byte)LeagueType.ThirdParty ? cacheRefService.GetTeamName3rd(teamId) : cacheRefService.GetTeamName(teamId);

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

            if (BetTypeHelper.IsNumberGame(data.BetType) ||
                BetTypeHelper.IsHappy5(data.BetType))
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
            Dictionary<int, string> betTypeNames)
        {
            var betTypeList = matchGroup.ToList();
            var firstWinLoss = betTypeList[0];
            var leagueName = leagueService.GetLeagueName(firstWinLoss.LeagueId, firstWinLoss.LeagueType, firstWinLoss.LeagueGroupId);
            var betTypeWinLoss = new List<BetTypeWinLossViewModel>();

            var total = new BetTypeWinLossViewModel();

            foreach (var winLoss in matchGroup)
            {
                var betTypeWinLossViewModel = new BetTypeWinLossViewModel(
                    winLoss.BetType,
                    betTypeNames[winLoss.BetType],
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
                leagueName,
                firstWinLoss.MatchId,
                firstWinLoss.Race,
                betTypeWinLoss,
                total,
                firstWinLoss,
                GetTeamName(firstWinLoss.HomeId, firstWinLoss.LeagueType),
                GetTeamName(firstWinLoss.AwayId, firstWinLoss.LeagueType));
        }

        internal BetTypeWinLossViewModel BuildWinLossTotal(IEnumerable<MatchWinLossDetailViewModel> matchWinLossDetail)
        {
            if (!matchWinLossDetail.Any())
            {
                return null;
            }

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
            BetTypeWinLossViewModel systemParlay,
            IList<BetTypeWinLossViewModel> parlays)
        {
            var grandTotal = new BetTypeWinLossViewModel()
                .Add(noParlayWinLossTotal)
                .Add(mixParlay)
                .Add(systemParlay);
            foreach (var parlay in parlays)
            {
                grandTotal = grandTotal.Add(parlay);
            }

            return grandTotal;
        }

        internal static bool IsParlayMatchWinLossDetail(int betType)
            => (BetTypeIdConstants.LeapGamingParlayMin <= betType && betType <= BetTypeIdConstants.LeapGamingParlayMax) ||
                (BetTypeIdConstants.Happy5LinkedGamesMin <= betType && betType <= BetTypeIdConstants.Happy5LinkedGamesMax);

    }
}