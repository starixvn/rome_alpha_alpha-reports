namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Utils;

    public class WinLossByProductParam
    {
        public WinLossByProductParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;
            UserSelectedProductIds = userSelectedProductIds;

            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;

            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel CustLevel { get; }
    }
}