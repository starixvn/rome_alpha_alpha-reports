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
    using RemotingRef;

    public interface WinLossByGameTypeDataAccess
    {
        IEnumerable<WinLossByGameTypeModel> GetWinLossByGameType(WinLossByGameTypeCriteria criteria);
    }

    public class WinLossByGameTypeDataAccessImpl : WinLossByGameTypeDataAccess
    {
        private readonly CacheRefService cacheRefService;

        public WinLossByGameTypeDataAccessImpl(CacheRefService cacheRefService)
        {
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossByGameTypeModel> GetWinLossByGameType(WinLossByGameTypeCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winlossByGameTypeData = new List<WinLossByGameTypeModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winlossByGameTypeData.Add(BuildWinLossByGameType(reader, criteria.CustLevel));
                        }
                    }
                }
            }

            return winlossByGameTypeData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossByGameTypeModel BuildWinLossByGameType(IDataReader reader, UserLevel reportLevel)
        {
            var gameTypeId = reader.ToInt("GameTypeID");

            var winLossByGameType = new WinLossByGameTypeModel(
                 gameTypeId: gameTypeId,
                 gameTypeName: cacheRefService.GetBetTypeName(gameTypeId),
                 turnover: reader.ToDecimal("TurnOver"),
                 memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")) : null,
                 agentNetWinLoss: reportLevel.IsNotSuperLevel() ?
                                   (reportLevel.IsMasterLevel() ?
                                   new NetWinLoss(reader.ToDecimal("AgTotal")) : new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm"))) : null,
                 masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ?
                                   (reportLevel.IsMasterLevel() ?
                                   new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")) : new NetWinLoss(reader.ToDecimal("MAgTotal"))) : null,
                 superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null);

            return winLossByGameType;
        }
    }
}