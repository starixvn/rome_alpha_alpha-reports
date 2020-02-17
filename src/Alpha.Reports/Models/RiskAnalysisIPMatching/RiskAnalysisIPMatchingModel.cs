namespace Alpha.Reports.Models
{
    public class RiskAnalysisIPMatchingModel
    {
        public RiskAnalysisIPMatchingModel() { }

        public RiskAnalysisIPMatchingModel(
            string ip,
            int memberCount,
            int betCount,
            decimal stake,
            decimal memberWinLoss,
            decimal superWinLoss,
            decimal company)
        {
            IP = ip;
            MemberCount = memberCount;
            BetCount = betCount;
            Stake = stake;
            MemberWinLoss = memberWinLoss;
            SuperWinLoss = superWinLoss;
            Company = company;
        }

        public string IP { get; }

        public int MemberCount { get; }

        public int BetCount { get; }

        public decimal Stake { get; }

        public decimal MemberWinLoss { get; }

        public decimal SuperWinLoss { get; }

        public decimal Company { get; }
    } 
}
