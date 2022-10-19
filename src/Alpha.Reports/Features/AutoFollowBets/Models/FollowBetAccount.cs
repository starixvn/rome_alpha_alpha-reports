namespace Alpha.Reports.Features.AutoFollowBets.Models
{
    public class FollowBetAccount
    {
        public FollowBetAccount()
        {
        }

        public FollowBetAccount(
            int custId,
            string username,
            int agentId,
            int dummyCustId,
            string dummyUsername)
        {
            CustId = custId;
            Username = username;
            AgentId = agentId;
            DummyCustId = dummyCustId;
            DummyUsername = dummyUsername;
        }

        public int CustId { get; }

        public string Username { get; }

        public int AgentId { get; }

        public int DummyCustId { get; }

        public string DummyUsername { get; }
    }
}
