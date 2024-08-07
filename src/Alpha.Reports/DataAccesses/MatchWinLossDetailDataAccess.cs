namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.MatchWinLossDetail;
    using Fanex.Data.Repository;
    using Models;
    using Models.MatchWinLossDetail;

    public interface MatchWinLossDetailDataAccess
    {
        IEnumerable<MatchWinLossDetailModel> GetData(MatchWinLossDetailCriteria criteria);
    }

    public class MatchWinLossDetailDataAccessImpl : MatchWinLossDetailDataAccess
    {
        private readonly List<int> ParlayBetGroupId = new List<int> { 3, 4 };
        private readonly IDynamicRepository repository;

        public MatchWinLossDetailDataAccessImpl(IDynamicRepository repository)
            => this.repository = repository;

        public IEnumerable<MatchWinLossDetailModel> GetData(MatchWinLossDetailCriteria criteria)
        {
            var isAgent = criteria.ReportLevel.IsAgentLevel();
            var isSuper = criteria.ReportLevel.IsSuperLevel();
            var rawdata = repository.Fetch<MatchWinLossDetailEntity>(criteria);

            return rawdata.Select(wl => BuildMatchWinLossDetailModel(wl, isAgent, isSuper)).ToList();
        }

        public MatchWinLossDetailModel BuildMatchWinLossDetailModel(MatchWinLossDetailEntity entity, bool isAgent, bool isSuper)
        {
            int.TryParse(entity.Race, out var race);

            return new MatchWinLossDetailModel(
                isParlay: ParlayBetGroupId.Contains(entity.BetGroup),
                matchId: entity.MatchId,
                homeId: entity.HomeId,
                awayId: entity.AwayId,
                betType: entity.BetType,
                liveIndicator: entity.LiveIndicator,
                leagueId: entity.LeagueId,
                leagueGroupId: entity.LeagueGroupId,
                leagueType: entity.LeagueType,
                race: race,
                matchCode: entity.MatchCode,
                isNeutral: entity.IsNeutral,
                showTime: entity.ShowTime,
                turnover: entity.Turnover,
                grossComm: entity.GrossComm,
                memberNetWinLoss: new NetWinLoss(entity.MemberWinLoss, entity.MemberComm),
                agentNetWinLoss: new NetWinLoss(entity.AgWinLoss, entity.AgComm),
                masterNetWinLoss: isAgent ? null : new NetWinLoss(entity.MAgWinLoss, entity.MAgComm),
                superNetWinLoss: isSuper ? new NetWinLoss(entity.SAgWinLoss, entity.SAgComm) : null);
        }
    }
}
