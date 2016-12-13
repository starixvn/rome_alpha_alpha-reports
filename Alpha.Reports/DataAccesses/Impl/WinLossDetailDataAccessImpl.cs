namespace Alpha.Reports.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.WinLossDetail;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinlossDetails;
    using Services;

    public class WinLossDetailDataAccessImpl : WinLossDetailDataAccess
    {
        public ReportData<WinLossDetail> GetData(WinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winLossDetails = new List<WinLossDetail>();
            var totalWinLoss = new WinLossDetail();

            using (var db = ObjectDbFactory.CreateObjectDbFunc(criteria.GetSettingKey()))
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

            return new ReportData<WinLossDetail>(
                winLossDetails,
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                totalWinLoss);
        }

        private WinLossDetail BuildWinLossDetail(IDataReader reader, UserLevel reportLevel)
        {
            var winLossDetail = new WinLossDetail(
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