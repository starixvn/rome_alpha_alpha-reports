namespace Alpha.Reports.Models.PokerGamesWinLossDetail
{
    public class PokerGamesWinLossDetailModel
    {
        public PokerGamesWinLossDetailModel()
        {
        }

        public PokerGamesWinLossDetailModel(
            int custId,
            string custName,
            decimal taxComm,
            decimal netTurnover,
            decimal turnover,
            int roundCount,
            PokerGamesWinLoss memberWinLoss,
            PokerGamesWinLoss agentWinLoss,
            PokerGamesWinLoss masterWinLoss,
            PokerGamesWinLoss superWinLoss)
        {
            CustId = custId;
            CustName = custName;
            TaxComm = taxComm;
            NetTurnover = netTurnover;
            Turnover = turnover;
            RoundCount = roundCount;
            MemberWinLoss = memberWinLoss;
            AgentWinLoss = agentWinLoss;
            MasterWinLoss = masterWinLoss;
            SuperWinLoss = superWinLoss;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal Turnover { get; }

        public decimal NetTurnover { get; }

        public decimal TaxComm { get; }

        public int RoundCount { get; }

        public PokerGamesWinLoss MemberWinLoss { get; }

        public PokerGamesWinLoss AgentWinLoss { get; }

        public PokerGamesWinLoss MasterWinLoss { get; }

        public PokerGamesWinLoss SuperWinLoss { get; }

        public decimal Company =>
            -((MemberWinLoss?.Total ?? 0)
            + (AgentWinLoss?.Total ?? 0)
            + (MasterWinLoss?.Total ?? 0)
            + (SuperWinLoss?.Total ?? 0));
    }
}