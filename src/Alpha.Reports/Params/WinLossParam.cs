namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class WinLossParam : SupportedHistoryReportParam
    {
        public WinLossParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            bool isHistoricReport) : base(fromDate, toDate, isHistoricReport)
        {
            CustId = custId;

            ReportLevel = reportLevel;

            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }
    }
}