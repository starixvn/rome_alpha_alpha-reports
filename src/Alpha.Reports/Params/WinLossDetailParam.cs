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
            int viewerId,
            string site,
            int currencyId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<int> userSelectedResourceIds,
            UserLevel reportLevel,
            string sortByPropertyName,
            bool isDescendingSort,
            bool isHistoricReport,
            string productDetailsXML,
            string username,
            bool isReward)
            : base(custId, viewerId, site, currencyId, fromDate, toDate, limitedDate, userSelectedProductIds, userSelectedResourceIds, reportLevel, isHistoricReport, productDetailsXML)
        {
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize;
            SortByPropertyName = sortByPropertyName;

            IsDescendingSort = isDescendingSort;

            Username = username;
            IsReward = isReward;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string SortByPropertyName { get; }

        public bool IsDescendingSort { get; }

        public string Username { get; }

        public bool IsReward { get; }
    }
}