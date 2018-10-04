namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Models;
    using Utils;

    public class WinLossAnalysisChartParam
    {
        public WinLossAnalysisChartParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel custLevel,
            IEnumerable<int> userSelectedExcludeProductIds)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today.AddMonths(-2).FirstDayOfMonth() : fromDate.FirstDayOfMonth();
            ToDate = (toDate == DateTime.MinValue || toDate.IsSameMonthWith(SystemTime.Today))
                ? SystemTime.Today : toDate.LastDayOfMonth();
            LimitedDateToShowInfo = limitedDate;
            UserSelectedProductIds = userSelectedProductIds;
            UserSelectedExcludeProductIds = userSelectedExcludeProductIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; set; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel CustLevel { get; }

        public DateTime LimitedDateToShowInfo { get; internal set; }

        public IEnumerable<int> UserSelectedExcludeProductIds { get; }
    }
}