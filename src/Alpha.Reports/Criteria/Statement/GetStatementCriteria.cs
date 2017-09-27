namespace Alpha.Reports.Criteria.Statement
{
    using System;
    using System.Collections;
    using Alpha.Reports.Services;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetStatementCriteria : CriteriaBase
    {
        public GetStatementCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            string storeMappingSuffix)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            BalanceUpDay = balanceUpDay;
            StoreMappingSuffix = storeMappingSuffix;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(Ignored = true)]
        public DateTime BalanceUpDay { get; }

        [SpParam(Ignored = true)]
        public string StoreMappingSuffix { get; }

        public override string GetSettingKey() => "Statement_Report_GetList" + StoreMappingSuffix;

        public override bool IsValid()
            => CustId > 0
            && FromDate <= ToDate;

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable
            {
                ["custid"] = CustId,
                ["fromdate"] = FromDate,
                ["todate"] = ToDate
            };

            if (string.Equals(StoreMappingSuffix, ReportDateServiceImpl.BetTrans14SPSuffix, StringComparison.OrdinalIgnoreCase))
            {
                result.Add("balanceupday", BalanceUpDay);
            }

            return result;
        }
    }
}
