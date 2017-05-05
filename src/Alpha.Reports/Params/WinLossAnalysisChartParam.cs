namespace Alpha.Reports.Params
{
    using Extensions;
    using Models;
    using System;
    using System.Collections.Generic;
    using Utils;

    public class WinLossAnalysisChartParam
    {
        public WinLossAnalysisChartParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today.AddMonths(-2).FirstDayOfMonth() : fromDate.FirstDayOfMonth();
            ToDate = (toDate == DateTime.MinValue || (toDate.Month == SystemTime.Today.Month && toDate.Year == SystemTime.Today.Year))
                ? SystemTime.Today : toDate.LastDayOfMonth();
            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; set; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel CustLevel { get; }
    }
}