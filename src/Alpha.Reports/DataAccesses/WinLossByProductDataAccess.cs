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
            var productId = Convert.IsDBNull(reader["ProductID"]) ? 0 : reader.ToInt("ProductID");
            var sportTypeId = Convert.IsDBNull(reader["SportType"]) ? 0 : reader.ToInt("SportType");
            var bettypeId = reader.ToInt("bettype");

            var winLossByBettype = new WinLossByBetTypeModel(
                 betTypeId: bettypeId,
                 betTypeName: cacheRefService.GetBetTypeName(bettypeId),
                 productId: productId,
                 sportTypeId: sportTypeId,
                 sportTypeName: sportTypeId == 0 ? string.Empty : cacheRefService.GetSportName(sportTypeId),
                 liveIndicator: reader.ToString("liveindicator") == "1",
                 betCount: bettypeId == BetTypeIdConstants.ChargeCommission ? (int?)null : reader.ToInt("NumberOfTickets"),
                 turnover: reader.ToDecimal("TurnOver"),
                 grossComm: reader.ToDecimal("GrossComm"),
                 memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")) : null,
                 agentNetWinLoss: NetWinLoss.ReadAgentNetWinLoss(reader, reportLevel),
                 masterNetWinLoss: NetWinLoss.ReadMasterNetWinLoss(reader, reportLevel),
                 superNetWinLoss: NetWinLoss.ReadSuperNetWinLoss(reader, reportLevel));

            return winLossByBettype;
        }
    }
}