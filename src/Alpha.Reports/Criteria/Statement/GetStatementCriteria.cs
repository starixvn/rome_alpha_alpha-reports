namespace Alpha.Reports.Criteria.Statement
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetStatementCriteria : CriteriaBase
    {
        public GetStatementCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string storeMappingSuffix)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            StoreMappingSuffix = storeMappingSuffix;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(Ignored = true)]
        public string StoreMappingSuffix { get; }

        public override string GetSettingKey() => "Statement_Report_GetList" + StoreMappingSuffix;

        public override bool IsValid()
            => CustId > 0
            && FromDate <= ToDate;
    }
}