namespace Alpha.Reports.Models.PokerGamesWinLossDetail
{
    public class PokerGamesWinLoss
    {
        public PokerGamesWinLoss()
        {
        }

        public PokerGamesWinLoss(
            decimal winloss,
            decimal insuranceGemsWinLoss,
            decimal comm)
        {
            WinLoss = winloss;
            InsuranceGemsWinLoss = insuranceGemsWinLoss;
            Comm = comm;
            Total = winloss + insuranceGemsWinLoss + comm;
        }

        public PokerGamesWinLoss(
            decimal? winloss,
            decimal? insuranceGemsWinLoss,
            decimal? comm,
            decimal total)
        {
            WinLoss = winloss;
            InsuranceGemsWinLoss = insuranceGemsWinLoss;
            Comm = comm;
            Total = total;
        }

        public decimal? WinLoss { get; private set; }

        public decimal? InsuranceGemsWinLoss { get; private set; }

        public decimal? Comm { get; private set; }

        public decimal Total { get; private set; }

        public static PokerGamesWinLoss Add(PokerGamesWinLoss winlossA, PokerGamesWinLoss winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return new PokerGamesWinLoss(
               winloss: winlossA?.WinLoss + winlossB?.WinLoss,
               insuranceGemsWinLoss: winlossA?.InsuranceGemsWinLoss + winlossB?.InsuranceGemsWinLoss,
               comm: winlossA?.Comm + winlossB?.Comm,
               total: winlossA.Total + winlossB.Total);
        }
    }
}
