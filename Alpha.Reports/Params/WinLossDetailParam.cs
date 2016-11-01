namespace Alpha.Reports.Params
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Models;

    public class WinLossDetailParam
    {
        public WinLossDetailParam(
            int custId,
            int pageIndex,
            int pageSize,            
            DateTime fromDate,
            DateTime toDate,
            string productIds,
            UserLevel userLevel)
        {
            CustId = custId;
            PageIndex = pageIndex;
            PageSize = pageSize;            
            FromDate = fromDate;
            ToDate = toDate;
            ProductIds = productIds.Split(',').Select(id => Convert.ToInt32(id));
            UserLevel = userLevel;
        }

        public int CustId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }       

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> ProductIds { get; }

        public UserLevel UserLevel { get; }

        public string SortedColumn { get; }

        public int SortDirection { get; }
    }
}
