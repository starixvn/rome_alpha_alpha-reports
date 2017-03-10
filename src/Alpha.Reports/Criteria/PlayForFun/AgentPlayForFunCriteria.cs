namespace Alpha.Reports.Criteria.PlayForFun
{
    using System;

    public class AgentPlayForFunCriteria : PlayForFunCriteria
    {
        public AgentPlayForFunCriteria(int custId, DateTime fromDate, DateTime toDate, int productId, int statusId)
            : base(custId, fromDate, toDate, productId, statusId)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_PlayForFun_Agent";
    }
}
