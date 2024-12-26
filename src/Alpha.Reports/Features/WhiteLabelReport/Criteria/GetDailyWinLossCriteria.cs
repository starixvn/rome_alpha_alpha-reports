namespace Alpha.Reports.Features.WhiteLabelReport.Criteria
{
    using System;

    internal class GetDailyWinLossCriteria : GetSettlementCriteria
    {
        public GetDailyWinLossCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string whiteLabels,
            string username) : base(custId, fromDate, toDate, whiteLabels, username)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_WhiteLabelWinLoss";
    }
}
