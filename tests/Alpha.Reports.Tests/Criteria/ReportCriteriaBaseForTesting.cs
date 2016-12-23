namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using Reports.Criteria;

    public class ReportCriteriaBaseForTesting : ReportCriteriaBase
    {
        public ReportCriteriaBaseForTesting(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay) : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
        }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}
