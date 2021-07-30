namespace Alpha.Reports.Features.SuperWinLossDetailByPT.Params
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
            string productDetailsXML,
            DateTime limitedDateToShowInfo)
        {
            CustId = custId;
            UserSelectedProductIds = userSelectedProductIds;
            UserSelectedResourceIds = userSelectedResourceIds;

            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;

            ProductDetailsXML = productDetailsXML;

            LimitedDateToShowInfo = limitedDateToShowInfo;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public IEnumerable<int> UserSelectedResourceIds { get; }

        public string ProductDetailsXML { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}