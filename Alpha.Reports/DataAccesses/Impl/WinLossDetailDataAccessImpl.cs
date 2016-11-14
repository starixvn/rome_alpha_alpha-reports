namespace Alpha.Reports.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.WinLossDetail;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Models;
    using Models.WinlossDetails;

    public class WinLossDetailDataAccessImpl : WinLossDetailDataAccess
    {
        public ReportData<WinLossDetail> GetReport(WinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winLossDetails = new List<WinLossDetail>();
            var totalWinLoss = new WinLossDetail();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        winLossDetails.Add(BuildWinLossDetail(reader, criteria.ReportLevel));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            totalWinLoss = BuildWinLossDetail(reader, criteria.ReportLevel);
                            break;
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
            NetWinLoss masterNetWinLoss = null;
            NetWinLoss superNetWinLoss = null;

            if (reportLevel.IsMasterOrSuperLevel())
            {
                masterNetWinLoss = new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm"));
            }

            if (reportLevel.IsSuperLevel())
            {
                superNetWinLoss = new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm"));
            }

            var winLossDetail = new WinLossDetail(
                custId: reader.ToInt("CustId"),
                custName: reader.ToString("CustName"),
                turnover: reader.ToDecimal("Turnover"),
                netTurnover: reader.ToDecimal("NetTurnover"),
                grossComm: reader.ToDecimal("GrossComm"),
                numberOfTickets: reader.ToInt("NumberOfTickets"),
                memberNetWinLoss: new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentNetWinLoss: new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm")),
                masterNetWinLoss: masterNetWinLoss,
                superNetWinLoss: superNetWinLoss);

            return winLossDetail;
        }
    }
}