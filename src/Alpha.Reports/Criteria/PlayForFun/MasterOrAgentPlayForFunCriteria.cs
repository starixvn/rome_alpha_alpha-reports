namespace Alpha.Reports.Criteria.PlayForFun
{
    using System;
    using Fanex.Data;
    using Models;

    public class MasterOrAgentPlayForFunCriteria : PlayForFunCriteria
    {
        public MasterOrAgentPlayForFunCriteria(int custId, DateTime fromDate, DateTime toDate, int productId, int statusId, UserLevel custLevel)
            : base(custId, fromDate, toDate, productId, statusId)
        {
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey() => CustLevel.IsSuperLevel()
            ? "Age_Rpt_PlayForFun_Master"
            : "Age_Rpt_PlayForFun_Agent";
    }
}
