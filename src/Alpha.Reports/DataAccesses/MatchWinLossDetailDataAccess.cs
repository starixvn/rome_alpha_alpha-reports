namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.MatchWinLossDetail;
    using Fanex.Common.Extensions;
    using Models;
    using Models.MatchWinLossDetail;

    public interface MatchWinLossDetailDataAccess
    {
        IEnumerable<MatchWinLossDetailModel> GetData(MatchWinLossDetailCriteria criteria);
    }

    public class MatchWinLossDetailDataAccessImpl : MatchWinLossDetailDataAccess
    {
        public IEnumerable<MatchWinLossDetailModel> GetData(MatchWinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var matchWinLoss = new List<MatchWinLossDetailModel>();
            var isAgent = criteria.ReportLevel.IsAgentLevel();
            var isSuper = criteria.ReportLevel.IsSuperLevel();

            using (var db = ReportObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            matchWinLoss.Add(BuildMatchWinLossDetailModel(reader, isAgent, isSuper));
                        }
                    }
                }
            }

            return matchWinLoss;
        }

        public MatchWinLossDetailModel BuildMatchWinLossDetailModel(IDataReader reader, bool isAgent, bool isSuper)
        {
            return new MatchWinLossDetailModel(
                isParlay: reader.ToInt("BetGroup") == 3,
                matchId: reader.ToInt("MatchId"),
                homeId: reader.ToInt("homeId"),
                awayId: reader.ToInt("awayId"),
                betType: reader.ToInt("betType"),
                liveIndicator: reader.ToInt("liveIndicator") == 1,
                leagueId: reader.ToInt("leagueId"),
                race: reader.ToInt("race"),
                matchCode: reader.ToString("matchCode"),
                isNeutral: reader.ToBool("isNeutral"),
                showTime: reader.ToString("showTime"),
                turnover: reader.ToDecimal("turnover"),
                grossComm: reader.ToDecimal("grossComm"),
                memberNetWinLoss: new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentNetWinLoss: new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm")),
                masterNetWinLoss: isAgent ? null : new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")),
                superNetWinLoss: isSuper ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null);
        }
    }
}
