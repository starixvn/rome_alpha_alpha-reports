namespace Alpha.Reports.DataAccesses.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.WinLossDetail;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinLossDetail;

    public class WinLossDetailDataAccessImpl : WinLossDetailDataAccess
    {
        public ReportData<WinLossDetailModel> GetData(WinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winLossDetails = new List<WinLossDetailModel>();
            var totalWinLoss = new WinLossDetailModel();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winLossDetails.Add(BuildWinLossDetail(reader, criteria.ReportLevel));
                        }

                        if (reader.NextResult() && reader.Read())
                        {
                            totalWinLoss = BuildWinLossDetail(reader, criteria.ReportLevel);
                        }
                    }
                }
            }

            return new ReportData<WinLossDetailModel>(
                winLossDetails,
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                totalWinLoss);
        }

        private WinLossDetailModel BuildWinLossDetail(IDataReader reader, UserLevel reportLevel)
        {
            var winLossDetail = new WinLossDetailModel(
                custId: reader.ToInt("CustId"),
                custName: reader.ToString("CustName"),
                turnover: reader.ToDecimal("Turnover"),
                netTurnover: reader.ToDecimal("NetTurnover"),
                grossComm: reader.ToDecimal("GrossComm"),
                numberOfTickets: reader.ToInt("NumberOfTickets"),
                memberNetWinLoss: new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentNetWinLoss: new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm")),
                masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ? new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")) : null,
                superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null);

            return winLossDetail;
        }
    }
}