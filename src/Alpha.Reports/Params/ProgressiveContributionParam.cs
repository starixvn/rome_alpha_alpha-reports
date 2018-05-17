namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Utils;

    public class ProgressiveContributionParam
    {
        public ProgressiveContributionParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel)
        {
            CustId = custId;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            UserSelectedProductIds = userSelectedProductIds;
            ReportLevel = reportLevel;
            LimitedDateToShowInfo = limitedDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}