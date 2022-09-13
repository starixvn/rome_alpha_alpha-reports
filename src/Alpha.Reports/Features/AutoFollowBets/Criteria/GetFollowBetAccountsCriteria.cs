namespace Alpha.Reports.Features.AutoFollowBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetFollowBetAccountsCriteria : CriteriaBase
    {
        public GetFollowBetAccountsCriteria(int agentId)
            => DummyAgentId = agentId;

        public int DummyAgentId { get; }

        public override string GetSettingKey()
            => "Age_Customer_FollowBetAccountListSelByAgent";

        public override bool IsValid()
            => DummyAgentId > 0;
    }
}
