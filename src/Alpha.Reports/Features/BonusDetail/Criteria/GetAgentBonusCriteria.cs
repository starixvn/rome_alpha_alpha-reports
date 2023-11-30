namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using System;

    internal class GetAgentBonusCriteria : GetBonusDetailCriteria
    {
        public GetAgentBonusCriteria(
            string username,
            DateTime fromDate,
            DateTime toDate,
            byte? statusId,
            bool? isComplained,
            int pageIndex,
            int pageSize,
            int agentId) : base(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize)
        {
            AgentId = agentId;
        }

        public int AgentId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_BonusDetail_Agent";

        public override bool IsValid()
            => base.IsValid() && AgentId > 0;
    }
}
