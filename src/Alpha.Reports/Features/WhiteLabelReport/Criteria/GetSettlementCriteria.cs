namespace Alpha.Reports.Features.WhiteLabelReport.Criteria
{
    using System;
    using System.Data.SqlTypes;
    using Fanex.Data.Repository;

    internal class GetSettlementCriteria : CriteriaBase
    {
        public GetSettlementCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string whiteLabels,
            string username)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            WhiteLabelList = whiteLabels;
            Username = username;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string WhiteLabelList { get; }

        public string Username { get; }

        public override string GetSettingKey() => "Age_Rpt_WhiteLabelSettlement";

        public override bool IsValid()
            => CustId > 0
            && FromDate <= ToDate
            && FromDate > SqlDateTime.MinValue.Value;
    }
}
