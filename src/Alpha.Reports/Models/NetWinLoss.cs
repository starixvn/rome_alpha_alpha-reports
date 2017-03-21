namespace Alpha.Reports.Models
{
    public class NetWinLoss
    {
        public NetWinLoss(decimal total)
        {
            Total = total;
        }

        public NetWinLoss(decimal winloss, decimal comm)
        {
            WinLoss = winloss;
            Comm = comm;
            Total = winloss + comm;
        }

        public NetWinLoss(decimal? winloss, decimal? comm, decimal total)
        {
            WinLoss = winloss;
            Comm = comm;
            Total = total;
        }

        public decimal? WinLoss { get; private set; }

        public decimal? Comm { get; private set; }

        public decimal Total { get; private set; }

        public static NetWinLoss Add(NetWinLoss winlossA, NetWinLoss winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return winlossA.Add(winlossB);
        }

        public NetWinLoss Add(NetWinLoss netWinLoss)
        {
            WinLoss += netWinLoss.WinLoss;
            Comm += netWinLoss.Comm;
            Total += netWinLoss.Total;

            return this;
        }
    }
}