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

        public int CustId { get; set; }

        public string Username { get; set; }

        public int AgentId { get; set; }

        public int DummyCustId { get; set; }

        public string DummyUsername { get; set; }
    }
}
