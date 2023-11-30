namespace Alpha.Reports.Criteria.Consolidation
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetConsolidationCriteria : CriteriaBase
    {
        public GetConsolidationCriteria(
            int custId,
            int currency,
            DateTime fromDate,
            DateTime toDate,
            byte reportType,
            DateTime balanceUpDate,
            DateTime moveBetsDate,
            string productIds,
            string productDetails,
            string subfix)
        {
            CustId = custId;
            Currency = currency;
            FromDate = fromDate;
            ToDate = toDate;
            ReportType = reportType;

            BalanceUpDate = balanceUpDate;
            MoveBetsDate = moveBetsDate;
            ProductIds = productIds;
            ProductDetails = productDetails;
            Subfix = subfix;
        }

        public int CustId { get; }

        public int Currency { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string ProductIds { get; }

        public byte ReportType { get; }

        [SpParam(SqlParamName = "BalanceUpDay")]
        public DateTime BalanceUpDate { get; }

        [SpParam(SqlParamName = "MoveBetsDay")]
        public DateTime MoveBetsDate { get; }

        public string ProductDetails { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey() => $"Age_Rpt_ConsolidatedGroup{Subfix}";

        public override bool IsValid()
            => CustId > 0
            && FromDate <= ToDate
            && !string.IsNullOrWhiteSpace(ProductIds);
    }
}
