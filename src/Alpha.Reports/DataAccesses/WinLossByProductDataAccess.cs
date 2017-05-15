namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Constants;
    using Criteria.WinLossByProductCriteria;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinlossByBetType;
    using RemotingRef;

    public interface WinLossByProductDataAccess
    {
        IEnumerable<WinLossByBetTypeModel> GetWinLossByBetType(WinLossByProductCriteria criteria);
    }

    public class WinLossByProductDataAccessImpl : WinLossByProductDataAccess
    {
        private readonly CacheRefService cacheRefService;

        public WinLossByProductDataAccessImpl(CacheRefService cacheRefService)
        {
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossByBetTypeModel> GetWinLossByBetType(WinLossByProductCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winlossByBettypeData = new List<WinLossByBetTypeModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winlossByBettypeData.Add(BuildWinLossByBetType(reader, criteria.CustLevel));
                        }
                    }
                }
            }

            return winlossByBettypeData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossByBetTypeModel BuildWinLossByBetType(IDataReader reader, UserLevel reportLevel)
        {
            var bettypeId = reader.ToInt("bettype");

            var winLossByBettype = new WinLossByBetTypeModel(
                 betTypeId: bettypeId,
                 betTypeName: cacheRefService.GetBetTypeName(bettypeId),
                 productId: reader.ToInt("ProductID"),
                 liveIndicator: reader.ToString("liveindicator") == "1",
                 betCount: bettypeId == BetTypeIdConstants.ChargeCommission ? (int?)null : reader.ToInt("NumberOfTickets"),
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