namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Criteria.WinLoss;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinlossByBettype;
    using Models.WinLossDetail;

    public interface WinLossByBettypeDataAccess
    {
        IEnumerable<WinlossByBettypeModel> GetWinLossByBettype(WinLossByBettypeCriteria criteria);
    }

    public class WinLossByBettypeDataAccessImpl : WinLossByBettypeDataAccess
    {
        public IEnumerable<WinlossByBettypeModel> GetWinLossByBettype(WinLossByBettypeCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winlossByBettypeData = new List<WinlossByBettypeModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winlossByBettypeData.Add(BuildWinLoss(reader, criteria.CustLevel));
                        }
                    }
                }
            }

            return winlossByBettypeData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinlossByBettypeModel BuildWinLoss(IDataReader reader, UserLevel reportLevel)
        {
            var winLossByBettype = new WinlossByBettypeModel(
                 custId: reader.ToInt("CustId"),
                 custName: reader.ToString("CustName"),
                 numberOfTickets: 102, ////reader.ToInt("NoOfTicket"),
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

            return winLossByBettype;
        }
    }
}