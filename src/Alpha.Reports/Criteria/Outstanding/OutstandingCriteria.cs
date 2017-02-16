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
        {
            if (CustLevel.IsSuperLevel())
            {
                return "Age_Rpt_Outstanding_Master";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_Outstanding_Agent";
            }

            return "Age_Rpt_Outstanding_Member";
        }

        public override bool IsValid()
        {
            return !CustLevel.IsMemberLevel();
        }
    }
}
