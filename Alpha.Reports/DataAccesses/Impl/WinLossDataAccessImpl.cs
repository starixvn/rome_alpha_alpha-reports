namespace Alpha.Reports.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Criteria.WinLoss;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Models;
    using Models.Winloss;
    using Models.WinlossDetails;

    public class WinLossDataAccessImpl : WinLossDataAccess
    {
        public IEnumerable<WinLoss> GetWinLosses(WinLossCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winLosses = new List<WinLoss>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winLosses.Add(BuildWinLoss(reader, criteria.ReportLevel));
                        }
                    }
                }
            }

            return winLosses;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLoss BuildWinLoss(IDataReader reader, UserLevel reportLevel)
        {
            var winLoss = new WinLoss(
                 custId: reader.ToInt("CustId"),
                 custName: reader.ToString("CustName"),
                 turnover: reader.ToDecimal("TurnOver"),
                 grossComm: reader.ToDecimal("GrossComm"),
                 memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")) : null,
                 agentNetWinLoss: reportLevel.IsNotSuperLevel() ?
                                   (reportLevel.IsMasterLevel() ? 
                                   new NetWinLoss(reader.ToDecimal("AgTotal")) : new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm"))) : null,
                 masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ?
                                   (reportLevel.IsMasterLevel() ? 
                                   new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")) : new NetWinLoss(reader.ToDecimal("MAgTotal"))) : null,
                 superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null);

            return winLoss;
        }
    }
}