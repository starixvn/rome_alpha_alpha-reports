namespace Alpha.Reports.Models.WinLossDetail
{
    using Newtonsoft.Json;

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
            bool isExistedWatchlist,
            decimal grossComm,
            decimal netTurnover,
            decimal turnover,
            int betCount,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss,
            decimal? rewardAmount,
            string rewardCurrency)
        {
            CustId = custId;
            CustName = custName;
            FirstName = firstName;
            LastName = lastName;
            IsExistedWatchlist = isExistedWatchlist;
            GrossComm = grossComm;
            NetTurnover = netTurnover;
            Turnover = turnover;
            BetCount = betCount;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
            RewardAmount = rewardAmount ?? 0;
            RewardCurrency = rewardCurrency;
        }

        public int CustId { get; }

        public string CustName { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                {
                    return string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                {
                    return $"{FirstName}/ {LastName}";
                }

                return $"{FirstName}{LastName}";
            }
        }

        public bool IsExistedWatchlist { get; }

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

        public decimal RewardAmount { get; }

        public string RewardCurrency { get; }
    }
}