namespace Alpha.Reports.Criteria.Outstanding
{
    using Alpha.Reports.Criteria.Outstanding;

    public class OutstandingAgentCriteria : OutstandingMemberCriteria
    {
        public OutstandingAgentCriteria(int custId, string productIds, byte levelType) : base(custId, productIds)
        {
            LevelType = levelType;
        }

        public byte LevelType { get; }

        public override string GetSettingKey() => "Age_Rpt_Outstanding_Agent";
    }
}
