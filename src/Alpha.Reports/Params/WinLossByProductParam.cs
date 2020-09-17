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
            IEnumerable<int> userSelectedResourceIds,
            UserLevel custLevel,
            string productDetailsXML)
        {
            CustId = custId;
            CustLevel = custLevel;
            UserSelectedProductIds = userSelectedProductIds;
            UserSelectedResourceIds = userSelectedResourceIds;

            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;

            ProductDetailsXML = productDetailsXML;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public IEnumerable<int> UserSelectedResourceIds { get; }

        public UserLevel CustLevel { get; }

        public string ProductDetailsXML { get; }
    }
}