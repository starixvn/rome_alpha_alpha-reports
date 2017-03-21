namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Criteria.WinLossAnalysis;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinLossAnalysis;
    using RemotingRef;

    public interface WinLossAnalysisDataAccess
    {
        IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisCriteria criteria);
    }

    public class WinLossAnalysisDataAccessImpl : WinLossAnalysisDataAccess
    {
        private readonly CacheRefService cacheRefService;

        public WinLossAnalysisDataAccessImpl(CacheRefService cacheRefService)
        {
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var data = new List<WinLossAnalysisModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data.Add(BuildWinLossAnalysisModel(reader, criteria));
                        }
                    }
                }
            }

            return data;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossAnalysisModel BuildWinLossAnalysisModel(IDataReader reader, WinLossAnalysisCriteria criteria)
        {
            var reportLevel = criteria.ReportLevel;

            var winLossAnalysis = new WinLossAnalysisModel(
                rollupKey: criteria.ReportType.Equals(3) ? cacheRefService.GetProductName(reader.ToInt("RollupKey")) : reader.ToString("RollupKey"), 
                turnover: reader.ToDecimal("TurnOver"),
                betCount: reader.ToInt("BetCount"),
                memberNetWinLoss: new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentNetWinLoss: new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm")),
                masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ? new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")) : null,
                superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null);

            return winLossAnalysis;
        }
    }
}