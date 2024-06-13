namespace Alpha.Reports.Criteria.Consolidation
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCurrencyCriteria : CriteriaBase
    {
        public GetCurrencyCriteria(int custId, string subfix)
        {
            CustId = custId;
            Subfix = subfix;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey()
            => $"Age_Rpt_ConsolidatedGroupSelCurrency{Subfix}";

        public override bool IsValid()
            => CustId > 0;
    }
}
