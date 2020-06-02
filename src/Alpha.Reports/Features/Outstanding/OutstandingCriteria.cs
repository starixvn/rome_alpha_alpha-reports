namespace Alpha.Reports.Criteria.Outstanding
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class OutstandingCriteria : CriteriaBase
    {
        public OutstandingCriteria(UserLevel custLevel, int custId, string productIds)
        {
            CustLevel = custLevel;
            CustId = custId;
            ProductIds = productIds;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public string ProductIds { get; }

        public override string GetSettingKey()
            => CustLevel.IsAgentLevel()
            ? "Age_Rpt_Outstanding_Member"
            : CustLevel.IsMasterLevel()
            ? "Age_Rpt_Outstanding_Agent"
            : "Age_Rpt_Outstanding_Master";

        public override bool IsValid() => !CustLevel.IsMemberLevel();
    }
}
