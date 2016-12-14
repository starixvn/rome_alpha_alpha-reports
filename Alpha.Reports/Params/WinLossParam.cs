namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Models;
    using Services;

    public class WinLossParam
    {
        public WinLossParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            bool isHistoricReport)
        {
            CustId = custId;

            IsHistoryReport = isHistoricReport;

            if (fromDate == DateTime.MinValue)
            {
                FromDate = isHistoricReport ? DateTime.Now.FirstDayOfMonth() : DateTimeAdapter.Now;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTimeAdapter.Now : toDate;

            ReportLevel = reportLevel;

            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public bool IsHistoryReport { get; }

        private string GetDefaultSortedProperty()
        {
            if (ReportLevel.IsSuperLevel())
            {
                return "SuperNetWinLoss.WinLoss";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "MasterNetWinLoss.WinLoss";
            }

            return "AgentNetWinLoss.WinLoss";
        }

        public bool IsValid()
        {
            if (IsHistoryReport)
            {
                return DateTimeAdapter.Now.AddYears(-3).FirstDayOfYear() <= FromDate
                    && FromDate <= ToDate
                    && ToDate.Year == FromDate.Year
                    && ToDate.Month == FromDate.Month;
            }

            return DateTimeAdapter.Now.AddMonths(-3).FirstDayOfMonth() <= FromDate
                   && FromDate <= ToDate;
        }
    }
}
