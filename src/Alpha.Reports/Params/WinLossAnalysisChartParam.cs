namespace Alpha.Reports.Params
{
    using Models;
    using System;
    using System.Collections.Generic;

    public class WinLossAnalysisChartParam
    {
        public WinLossAnalysisChartParam(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate;
            ToDate = toDate;
            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; set; }

        public IEnumerable<int> UserSelectedProductIds { get; }
    }
}