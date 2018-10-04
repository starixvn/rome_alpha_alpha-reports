namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Models;

    public class WinLossDetailParam : WinLossParam
    {
        public WinLossDetailParam(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            string sortByPropertyName,
            bool isDescendingSort,
            bool isHistoricReport,
            IEnumerable<int> userSelectedExcludeProductIds)
            : base(custId, fromDate, toDate, limitedDate, userSelectedProductIds, reportLevel, isHistoricReport, userSelectedExcludeProductIds)
        {
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize;
            SortByPropertyName = sortByPropertyName;

            IsDescendingSort = isDescendingSort;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string SortByPropertyName { get; }

        public bool IsDescendingSort { get; }
    }
}