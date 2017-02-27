namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Criteria.WinLossByProductCriteria;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinlossByBetType;
    using Models.WinLossByProduct;
    using RemotingRef;

    public interface WinLossByProductDataAccess
    {
        IEnumerable<WinLossByProductModel> GetWinLossByProduct(WinLossByProductCriteria criteria);
    }

    public class WinLossByProductDataAccessImpl : WinLossByProductDataAccess
    {
        private readonly CacheRefService cacheRefService;

        public WinLossByProductDataAccessImpl(CacheRefService cacheRefService)
        {
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossByProductModel> GetWinLossByProduct(WinLossByProductCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winlossByProductData = new List<WinLossByProductModel>()
            {
                new WinLossByProductModel(22, "Casino Games", false, 45, 1202, 5004, null, null, new NetWinLoss(400), new NetWinLoss(123, 564), Enumerable.Empty<WinLossByBetTypeModel>()),
                new WinLossByProductModel(23, "NG2", false, 89, 2655, 6, null, null, new NetWinLoss(307), new NetWinLoss(458, 569), Enumerable.Empty<WinLossByBetTypeModel>()),
                new WinLossByProductModel(24, "Virtual Sport", false, 56, 1560, 637, null, null, new NetWinLoss(232), new NetWinLoss(233, 564), Enumerable.Empty<WinLossByBetTypeModel>()),
                new WinLossByProductModel(25, "Gold Deluxe", false, 105, 1236, 236, null, null, new NetWinLoss(723), new NetWinLoss(458, 459), Enumerable.Empty<WinLossByBetTypeModel>()),
                new WinLossByProductModel(26, "Macau Games", false, 205, 1355, 156, null, null, new NetWinLoss(122), new NetWinLoss(423, 564), Enumerable.Empty<WinLossByBetTypeModel>())
            };

            ////using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            ////{
            ////    using (var reader = db.ExecuteReader(criteria))
            ////    {
            ////        if (reader != null)
            ////        {
            ////            while (reader.Read())
            ////            {
            ////                winlossByProductData.Add(BuildWinLossByProductModel(reader, criteria.CustLevel));
            ////            }
            ////        }
            ////    }
            ////}

            return winlossByProductData;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossByProductModel BuildWinLossByProductModel(IDataReader reader, UserLevel reportLevel)
        {
            var productId = reader.ToInt("ProductID");

            var winlossByProduct = new WinLossByProductModel(
                 productId: productId,
                 productName: cacheRefService.GetProductName(productId),
                 liveIndicator: reader.ToBool("liveindicator"),
                 betCount: reader.ToInt("NumberOfTickets"),
                 turnover: reader.ToDecimal("TurnOver"),
                 grossComm: reader.ToDecimal("GrossComm"),
                 memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")) : null,
                 agentNetWinLoss: reportLevel.IsNotSuperLevel() ?
                                   (reportLevel.IsMasterLevel() ?
                                   new NetWinLoss(reader.ToDecimal("AgTotal")) : new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm"))) : null,
                 masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ?
                                   (reportLevel.IsMasterLevel() ?
                                   new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")) : new NetWinLoss(reader.ToDecimal("MAgTotal"))) : null,
                 superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null,
                 winLossByBetTypeData: Enumerable.Empty<WinLossByBetTypeModel>());

            return winlossByProduct;
        }
    }
}