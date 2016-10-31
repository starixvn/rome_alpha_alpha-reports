namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;

    public abstract class BaseWinLossDetailCriteria : ReportCriteriaBase
    {
        protected BaseWinLossDetailCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            string productIds,
            int pageSize,
            int pageIndex) :
            base(custId, pageIndex, pageSize, fromDate, toDate)
        {
            BalanceUpDay = balanceUpDay;
            ProductIds = productIds;
        }

        public DateTime BalanceUpDay { get; }

        public string ProductIds { get; }

        public override bool IsValid() => FromDate <= ToDate && CustId > 0;
    }
}
