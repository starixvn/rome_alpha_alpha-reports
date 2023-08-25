using System;
using Alpha.Reports.Services;

namespace Alpha.Reports.Criteria.Statement
{
    public class GetStatementBetTrans14Criteria : GetStatementCriteria
    {
        public GetStatementBetTrans14Criteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay)
            : base(custId, fromDate, toDate, ReportDateServiceImpl.BetTrans14SPSuffix)
        {
            BalanceUpDay = balanceUpDay;
        }

        public DateTime BalanceUpDay { get; }

        public override string GetSettingKey() => "Statement_Report_GetList_14";
    }
}