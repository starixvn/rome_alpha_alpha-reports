namespace Alpha.Reports.Params.Consolidation
{
    using System;
    using System.Collections.Generic;

    public class GetConsolidationParam
    {
        public GetConsolidationParam(
            int custId,
            int currency,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> productIds,
            IEnumerable<int> excludeProductIds)
        {
            CustId = custId;
            Currency = currency;
            FromDate = fromDate;
            ToDate = toDate;
            ProductIds = productIds;
            ExcludeProductIds = excludeProductIds;
        }

        public int CustId { get; }

        public int Currency { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> ProductIds { get; }

        public IEnumerable<int> ExcludeProductIds { get; }
    }
}
