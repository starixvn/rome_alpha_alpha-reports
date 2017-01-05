namespace Alpha.Reports.Criteria.Outstanding
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class OutstandingCriteria : CriteriaBase
    {
        public OutstandingCriteria(UserLevel custLevel, int custId)
        {
            CustLevel = custLevel;
            CustId = custId;
        }

        public int CustId { get; private set; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsSuperLevel())
            {
                return "Age_Rpt_Outstanding_SAg";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_Outstanding_MAg";
            }

            return "Age_Rpt_Outstanding_Ag";
        }

        public override bool IsValid()
        {
            return CustLevel.IsMemberLevel() ? false : true;
        }
    }
}
