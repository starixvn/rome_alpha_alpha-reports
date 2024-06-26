namespace Alpha.Reports.Criteria.Consolidation
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params.Consolidation;

    public class GetConsolidationCriteria : CriteriaBase
    {
        public GetConsolidationCriteria(
            GetConsolidationParam param,
            DateTime balanceUpDate,
            DateTime moveBetsDate,
            byte reportType,
            string productDetails,
            string subfix)
            : this(
                  param.CustId,
                  param.Currency,
                  param.FromDate,
                  param.ToDate,
                  reportType,
                  balanceUpDate,
                  moveBetsDate,
                  param.ProductIds,
                  productDetails,
                  subfix)
        {
        }

        public GetConsolidationCriteria(
            int custId,
            int currency,
            DateTime fromDate,
            DateTime toDate,
            byte reportType,
            DateTime balanceUpDate,
            DateTime moveBetsDate,
            IEnumerable<int> productIds,
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
            ProductIds = productIds.JoinWithComma();
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
