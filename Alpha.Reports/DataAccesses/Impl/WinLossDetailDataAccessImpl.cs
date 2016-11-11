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
        public Tuple<IEnumerable<WinLossDetail>, WinLossDetail, int> GetReport(WinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var bodyRows = new List<WinLossDetail>();
            var totalRow = new WinLossDetail();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        bodyRows.Add(BuildWinLossDetail(reader, criteria.ReportLevel));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            totalRow = BuildWinLossDetail(reader, criteria.ReportLevel);
                        }
                    }
                }
            }

            return new Tuple<IEnumerable<WinLossDetail>, WinLossDetail, int>(bodyRows, totalRow, criteria.TotalRecords);
        }

        private WinLossDetail BuildWinLossDetail(IDataReader reader, UserLevel reportLevel)
        {
            CustWinLoss masterWinLoss = null;
            CustWinLoss superWinLoss = null;

            if (reportLevel.IsMasterOrSuperLevel())
            {
                masterWinLoss = new CustWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm"));
            }

            if (reportLevel.IsSuperLevel())
            {
                superWinLoss = new CustWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm"));
            }

            var winLossDetail = new WinLossDetail(
                custId: reader.ToInt("CustId"),
                custName: reader.ToString("CustName"),
                turnover: reader.ToDecimal("Turnover"),
                netTurnover: reader.ToDecimal("NetTurnover"),
                grossComm: reader.ToDecimal("GrossComm"),
                numberOfTickets: reader.ToInt("NumberOfTickets"),
                memberWinLoss: new CustWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentWinLoss: new CustWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm")),
                masterWinLoss: masterWinLoss,
                superWinLoss: superWinLoss);

            return winLossDetail;
        }
    }
}