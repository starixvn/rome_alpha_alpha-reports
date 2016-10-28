namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;

    public class WinLossDetailParam
    {
        public WinLossDetailParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> productIds,
            int pageSize,
            int pageIndex)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            ProductIds = productIds;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> ProductIds { get; }

        public int PageSize { get; }

        public int PageIndex { get; }
    }
}
