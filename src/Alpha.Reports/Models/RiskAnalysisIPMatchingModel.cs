namespace Alpha.Reports.Models
{
    public class RiskAnalysisIPMatchingModel
    {
        public RiskAnalysisIPMatchingModel() { }

        public RiskAnalysisIPMatchingModel(
            string ip,
            int memberCount,
            int betCount,
            decimal memberTurnover,
            decimal memberWinLoss,
            decimal superWinLoss,
            decimal company)
        {
            IP = ip;
            MemberCount = memberCount;
            BetCount = betCount;
            MemberTurnover = memberTurnover;
            MemberWinLoss = memberWinLoss;
            SuperWinLoss = superWinLoss;
            Company = company;
        }

        public string IP { get; }

        public int MemberCount { get; }

        public int BetCount { get; }

        public decimal MemberTurnover { get; }

        public decimal MemberWinLoss { get; }

        public decimal SuperWinLoss { get; }

        public decimal Company { get; }
    } 
}
