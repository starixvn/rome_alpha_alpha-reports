namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Criteria.WinLossDetail;
    using Fanex.Common.Extensions;
    using Models;
    using Models.WinLossDetail;

    public interface WinLossDetailDataAccess
    {
        PagedReportData<WinLossDetailModel> GetData(WinLossDetailCriteria criteria);

        Task<PagedReportData<WinLossDetailModel>> GetDataAsync(WinLossDetailCriteria criteria);
    }

    public class WinLossDetailDataAccessImpl : WinLossDetailDataAccess
    {
        public PagedReportData<WinLossDetailModel> GetData(WinLossDetailCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var winLossDetails = new List<WinLossDetailModel>();
            var totalWinLoss = new WinLossDetailModel();
            if(criteria.IsValid())
            {
                using (var db = ReportObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
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
            }            

            return new PagedReportData<WinLossDetailModel>(
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
                firstName: null,//reader.ToString("FirstName"),
                lastName: null,// reader.ToString("LastName"),
                turnover: reader.ToDecimal("Turnover"),
                netTurnover: reader.ToDecimal("NetTurnover"),
                grossComm: reader.ToDecimal("GrossComm"),
                betCount: reader.ToInt("NumberOfTickets"),
                memberNetWinLoss: new NetWinLoss(reader.ToDecimal("MemberWinLoss"), reader.ToDecimal("MemberComm")),
                agentNetWinLoss: new NetWinLoss(reader.ToDecimal("AgWinLoss"), reader.ToDecimal("AgComm")),
                masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ? new NetWinLoss(reader.ToDecimal("MAgWinLoss"), reader.ToDecimal("MAgComm")) : null,
                superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(reader.ToDecimal("SAgWinLoss"), reader.ToDecimal("SAgComm")) : null);

            return winLossDetail;
        }

        public async Task<PagedReportData<WinLossDetailModel>> GetDataAsync(WinLossDetailCriteria criteria)
        {
            var winLossDetails = new List<WinLossDetailModel>();
            var totalWinLoss = new WinLossDetailModel();

            if (criteria.IsValid())
            {
                using (var db = ReportObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
                {
                    using (var reader = await db.ExecuteReaderAsync(criteria))
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

            return new PagedReportData<WinLossDetailModel>(
                winLossDetails,
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                totalWinLoss);
        }
    }
}