namespace Alpha.Reports.Criteria
{
    using System;

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
            string productIds)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            ProductIds = productIds;
        }

        public string ProductIds { get; }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => base.IsValid();
    }
}
