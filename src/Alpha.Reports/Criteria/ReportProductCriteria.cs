namespace Alpha.Reports.Criteria
{
    using System;

    public class ReportProductCriteria : ReportCriteriaBase
    {
        public ReportProductCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds) : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            ProductIds = productIds;
        }

        public string ProductIds { get; }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => base.IsValid() && !string.IsNullOrWhiteSpace(ProductIds);
    }
}