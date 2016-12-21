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
                FromDate = isHistoricReport ? DateTimeManager.Now.FirstDayOfMonth() : DateTimeManager.Now;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTimeManager.Now : toDate;

            ReportLevel = reportLevel;

            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public bool IsHistoryReport { get; }

        public bool IsValid()
        {
            if (IsHistoryReport)
            {
                return DateTimeManager.Now.AddYears(-3).FirstDayOfYear() <= FromDate
                    && FromDate <= ToDate
                    && ToDate.Year == FromDate.Year
                    && ToDate.Month == FromDate.Month;
            }

            return DateTimeManager.Now.AddMonths(-3).FirstDayOfMonth() <= FromDate
                   && FromDate <= ToDate;
        }
    }
}
