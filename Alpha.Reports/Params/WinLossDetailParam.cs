namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
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
            UserLevel userLevel,
            string sortedColumn,
            int sortDirection)
        {
            CustId = custId;
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageIndex;
            FromDate = fromDate == DateTime.MinValue ? DateTime.Now : fromDate;
            ToDate = toDate == DateTime.MinValue ? DateTime.Now : fromDate;
            ProductIds = productIds.Split(',').Select(id => Convert.ToInt32(id));
            UserLevel = userLevel;
            SortedColumn = sortedColumn;
            SortDirection = sortDirection;
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
