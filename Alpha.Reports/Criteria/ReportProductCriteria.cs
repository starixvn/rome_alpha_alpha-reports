namespace Alpha.Reports.Criteria
{
    using System;
    using System.Collections.Generic;

    public class ReportProductCriteria : ReportCriteriaBase
    {
        public ReportProductCriteria(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            ProductIds = productIds == null ? string.Empty : string.Join(",", productIds);
        }

        public string ProductIds { get; }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => base.IsValid()
            && !string.IsNullOrWhiteSpace(ProductIds);
    }
}
