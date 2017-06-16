namespace Alpha.Reports.Criteria.Consolidation
{
    using Fanex.Data.Repository;

    public class GetCurrencyCriteria : CriteriaBase
    {
        public GetCurrencyCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_ConsolidatedGroupSelCurrency";

        public override bool IsValid()
            => CustId > 0;
    }
}
