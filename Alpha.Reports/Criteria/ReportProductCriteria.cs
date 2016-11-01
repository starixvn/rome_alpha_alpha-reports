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
            IEnumerable<int> productIdList)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            ProductIdList = productIdList;
        }

        private IEnumerable<int> ProductIdList { get; }

        public string ProductIds => string.Join(",", ProductIdList);

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => base.IsValid();
    }
}
