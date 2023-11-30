namespace Alpha.Reports.Criteria.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class MatchWinLossDetailCriteria : CriteriaBase
    {
        private const int LeagueIdItemsSplitted = 2;

        public MatchWinLossDetailCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            string productIds,
            UserLevel reportLevel,
            IEnumerable<string> leagues)
        {
            ReportLevel = reportLevel;
            Leagues = leagues;

            ProductIds = productIds;
            CustId = custId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            MoveBetsDay = moveBetsDay;
        }

        [SpParam(Ignored = true)]
        public IEnumerable<string> Leagues { get; }
        
        public string LeagueIds
        {
            get
            {
                var ids = new List<int>();
                foreach (var league in Leagues)
                {
                    var items = league.Split('_');
                    if (items.Length == LeagueIdItemsSplitted)
                    {
                        var leagueId = Convert.ToInt32(items[0]);
                        var leagueType = Convert.ToInt32(items[1]);
                        if (leagueType == (int)LeagueType.Self)
                        {
                            ids.Add(leagueId);
                        }
                    }
                }

                return ids.JoinWithComma();
            }
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public string ProductIds { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public bool IsGetAll => string.IsNullOrEmpty(LeagueIds);

        public override string GetSettingKey()
            => ReportLevel.IsAgentLevel()
            ? "Age_Rpt_MatchWinLossDetail_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_MatchWinLossDetail_Agent"
            : "Age_Rpt_MatchWinlossDetail_Master";

        public override bool IsValid() =>
            CustId > 0
            && FromDate <= ToDate
            && !string.IsNullOrWhiteSpace(ProductIds);
    }
}
