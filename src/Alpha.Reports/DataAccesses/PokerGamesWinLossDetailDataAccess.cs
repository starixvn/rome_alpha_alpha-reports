namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.PokerGamesWinLossDetail;
    using Fanex.Common.Extensions;
    using Models;
    using Models.PokerGamesWinLossDetail;

    public interface PokerGamesWinLossDetailDataAccess
    {
        PagedReportData<PokerGamesWinLossDetailModel> GetData(PokerGamesWinLossDetailCriteria criteria);
    }

    public class PokerGamesWinLossDetailDataAccessImpl : PokerGamesWinLossDetailDataAccess
    {
        public PagedReportData<PokerGamesWinLossDetailModel> GetData(PokerGamesWinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var pokerGameswinLossDetails = new List<PokerGamesWinLossDetailModel>();
            var totalWinLoss = new PokerGamesWinLossDetailModel();
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
                                pokerGameswinLossDetails.Add(BuildWinLossDetail(reader, criteria.ReportLevel));
                            }

                            if (reader.NextResult() && reader.Read())
                            {
                                totalWinLoss = BuildWinLossDetail(reader, criteria.ReportLevel);
                            }
                        }
                    }
                }
            }

            return new PagedReportData<PokerGamesWinLossDetailModel>(
                pokerGameswinLossDetails,
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                totalWinLoss);
        }

        private PokerGamesWinLossDetailModel BuildWinLossDetail(IDataReader reader, UserLevel reportLevel)
        {
            var winLossDetail = new PokerGamesWinLossDetailModel(
                custId: reader.ToInt("CustId"),
                custName: reader.ToString("CustName"),
                turnover: reader.ToDecimal("Turnover"),
                netTurnover: reader.ToDecimal("NetTurnover"),
                taxComm: reader.ToDecimal("TaxComm"),
                roundCount: reader.ToInt("RoundCount"),
                memberWinLoss: new PokerGamesWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("InsGemMemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentWinLoss: new PokerGamesWinLoss(0, reader.ToDecimal("InsGemAgWinLoss"), reader.ToDecimal("AgComm")),
                masterWinLoss: reportLevel.IsMasterOrSuperLevel() ? new PokerGamesWinLoss(0, reader.ToDecimal("InsGemMAgWinLoss"), reader.ToDecimal("MAgComm")) : null,
                superWinLoss: reportLevel.IsSuperLevel() ? new PokerGamesWinLoss(0, reader.ToDecimal("InsGemSAgWinLoss"), reader.ToDecimal("SAgComm")) : null);

            return winLossDetail;
        }
    }
}