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
            IEnumerable<int> productIds)
        {
            CustId = custId;
            Currency = currency;
            FromDate = fromDate;
            ToDate = toDate;
            ProductIds = productIds;
        }

        public int CustId { get; }

        public int Currency { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> ProductIds { get; }
    }
}
