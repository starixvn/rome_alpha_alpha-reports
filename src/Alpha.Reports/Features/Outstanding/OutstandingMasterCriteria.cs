namespace Alpha.Reports.Criteria.Outstanding
{
    public class OutstandingMasterCriteria : OutstandingAgentCriteria
    {
        public OutstandingMasterCriteria(int custId, string productIds, byte levelType)
            : base(custId, productIds, levelType)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_Outstanding_Master";
    }
}
