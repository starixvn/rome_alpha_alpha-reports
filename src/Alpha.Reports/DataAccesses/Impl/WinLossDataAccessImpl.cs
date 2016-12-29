namespace Alpha.Reports.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Criteria.WinLoss;
    using Fanex.Common.Extensions;
    using Models;
    using Models.Winloss;
    using Models.WinLossDetail;

    public class WinLossDataAccessImpl : WinLossDataAccess
    {
        public IEnumerable<WinLossModel> GetWinLoss(WinLossCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winlossData = new List<WinLossModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winlossData.Add(BuildWinLoss(reader, criteria.ReportLevel));
                        }
                    }
                }
            }

            return winlossData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossModel BuildWinLoss(IDataReader reader, UserLevel reportLevel)
        {
            var winLoss = new WinLossModel(
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