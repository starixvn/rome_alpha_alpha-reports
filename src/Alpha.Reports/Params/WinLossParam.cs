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
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            bool isHistoricReport,
            IEnumerable<int> userSelectedExcludeProductIds) : base(fromDate, toDate, limitedDate, isHistoricReport)
        {
            CustId = custId;

            ReportLevel = reportLevel;

            UserSelectedProductIds = userSelectedProductIds;

            UserSelectedExcludeProductIds = userSelectedExcludeProductIds;
        }

        public int CustId { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public IEnumerable<int> UserSelectedExcludeProductIds { get; }
    }
}