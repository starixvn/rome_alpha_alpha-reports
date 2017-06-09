namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Utils;

    public class WinLossAnalysisParam
    {
        public WinLossAnalysisParam(
          int custId,
          int reportType,
          DateTime fromDate,
          DateTime toDate,
          IEnumerable<int> userSelectedProductIds,
          UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;

            ReportTypeId = reportType;

            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;

            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public int ReportTypeId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel CustLevel { get; }
    }
}