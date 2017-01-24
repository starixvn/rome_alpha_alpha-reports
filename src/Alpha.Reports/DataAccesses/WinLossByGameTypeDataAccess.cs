namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Criteria.WinLossByGameType;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinlossByGameType;

    public interface WinLossByGameTypeDataAccess
    {
        IEnumerable<WinLossByGameTypeModel> GetWinLossByGameType(WinLossByGameTypeCriteria criteria);
    }

    public class WinLossByGameTypeDataAccessImpl : WinLossByGameTypeDataAccess
    {
        public IEnumerable<WinLossByGameTypeModel> GetWinLossByGameType(WinLossByGameTypeCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winlossByBettypeData = new List<WinLossByGameTypeModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winlossByBettypeData.Add(BuildWinLossByGameType(reader, criteria.CustLevel));
                        }
                    }
                }
            }

            return winlossByBettypeData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossByGameTypeModel BuildWinLossByGameType(IDataReader reader, UserLevel reportLevel)
        {
            var winLossByBettype = new WinLossByGameTypeModel(
                 gameTypeId: reader.ToInt("CustId"),
                 gameTypeName: reader.ToString("CustName"),
                 turnover: reader.ToDecimal("TurnOver"),
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