namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using Alpha.Reports.Features.BonusDetail.Params;

    internal class GetAgentBonusCriteria : GetBonusDetailCriteria
    {
        public GetAgentBonusCriteria(BonusDetailParam param) : base(param)
        {
            AgentId = param.ViewerId;
        }

        public int AgentId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_BonusDetail_Agent";

        public override bool IsValid()
            => base.IsValid() && AgentId > 0;
    }
}
