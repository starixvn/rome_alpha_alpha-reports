namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public abstract class BaseWinLossDetailCriteria : CriteriaBase
    {
        protected BaseWinLossDetailCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            string productIds,
            int pageSize,
            int pageIndex)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            BalanceUpDay = balanceUpDay;
            ProductIds = productIds;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDay { get; }

        public string ProductIds { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        [SpParam(SqlParamName = "TotalRecords")]
        public int TotalRows { get; protected set; }

        public override bool IsValid() => FromDate <= ToDate && CustId > 0;
    }
}
