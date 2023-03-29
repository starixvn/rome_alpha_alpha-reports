using Newtonsoft.Json;

namespace Alpha.Reports.Models.WinLossDetail
{
    public class WinLossDetailModel
    {
        public WinLossDetailModel()
        {
        }

        public WinLossDetailModel(
            int custId,
            string custName,
            string firstName,
            string lastName,
            decimal grossComm,
            decimal netTurnover,
            decimal turnover,
            int betCount,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss)
        {
            CustId = custId;
            CustName = custName;
            FirstName = firstName;
            LastName = lastName;
            GrossComm = grossComm;
            NetTurnover = netTurnover;
            Turnover = turnover;
            BetCount = betCount;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
        }

        public int CustId { get; }

        public string CustName { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public decimal Turnover { get; }

        public decimal NetTurnover { get; }

        public decimal GrossComm { get; }

        public int BetCount { get; }

        public NetWinLoss MemberNetWinLoss { get; }

        public NetWinLoss AgentNetWinLoss { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NetWinLoss MasterNetWinLoss { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NetWinLoss SuperNetWinLoss { get; }

        public decimal Company =>
            -((MemberNetWinLoss?.Total ?? 0)
            + (AgentNetWinLoss?.Total ?? 0)
            + (MasterNetWinLoss?.Total ?? 0)
            + (SuperNetWinLoss?.Total ?? 0));
    }
}