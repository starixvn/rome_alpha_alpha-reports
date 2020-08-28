namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Criteria.WinLossByProductCriteria;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinlossByBetType;
    using Services;

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
            if (criteria.IsValid())
            {
                using (var db = ReportObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
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
            }

            return winlossByBettypeData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossByBetTypeModel BuildWinLossByBetType(IDataReader reader, UserLevel reportLevel)
        {
            var productId = Convert.IsDBNull(reader["ProductID"]) ? 0 : reader.ToInt("ProductID");
            var sportTypeId = Convert.IsDBNull(reader["SportType"]) ? 0 : reader.ToInt("SportType");
            var bettypeId = reader.ToInt("bettype");
            var resourceId = Convert.IsDBNull(reader["ResourceId"]) ? 0 : reader.ToInt("ResourceId");

            var winLossByBettype = new WinLossByBetTypeModel(
                 betTypeId: bettypeId,
                 betTypeName: cacheRefService.GetBetTypeName(bettypeId),
                 productId: productId,
                 sportTypeId: sportTypeId,
                 sportTypeName: GetSportTypeName(resourceId, sportTypeId),
                 liveIndicator: reader.ToString("liveindicator") == "1",
                 betCount: reader.ToInt("NumberOfTickets"),
                 turnover: reader.ToDecimal("TurnOver"),
                 grossComm: reader.ToDecimal("GrossComm"),
                 memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")) : null,
                 agentNetWinLoss: NetWinLoss.ReadAgentNetWinLoss(reader, reportLevel),
                 masterNetWinLoss: NetWinLoss.ReadMasterNetWinLoss(reader, reportLevel),
                 superNetWinLoss: NetWinLoss.ReadSuperNetWinLoss(reader, reportLevel));

            return winLossByBettype;
        }

        private string GetSportTypeName(int resourceId, int sportTypeId)
        {
            if (resourceId > 0)
            {
                return cacheRefService.GetProductDetailName(resourceId);
            }

            return sportTypeId == 0 ? string.Empty : cacheRefService.GetSportName(sportTypeId);
        }
    }
}