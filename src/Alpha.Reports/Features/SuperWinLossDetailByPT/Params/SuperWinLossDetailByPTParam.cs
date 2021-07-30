namespace Alpha.Reports.Params.SuperWinLossDetailByPT
{
    using System;
    using System.Collections.Generic;
    using Utils;

    public class SuperWinLossDetailByPTParam
    {
        public SuperWinLossDetailByPTParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<int> userSelectedResourceIds,
            string productDetailsXML)
        {
            CustId = custId;
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

        public string ProductDetailsXML { get; }
    }
}