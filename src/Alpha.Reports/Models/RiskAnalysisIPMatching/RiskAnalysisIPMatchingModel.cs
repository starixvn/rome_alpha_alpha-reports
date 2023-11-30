namespace Alpha.Reports.Models
{
    public class RiskAnalysisIPMatchingModel
    {
        public RiskAnalysisIPMatchingModel()
        {
        }

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

        public string IP { get; set; }

        public int MemberCount { get; set; }

        public int BetCount { get; set; }

        public decimal Stake { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal SuperWinLoss { get; set; }

        public decimal Company { get; set; }
    } 
}
