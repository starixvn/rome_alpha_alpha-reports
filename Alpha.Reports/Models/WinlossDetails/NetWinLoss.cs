namespace Alpha.Reports.Models.WinlossDetails
{
    public class NetWinLoss
    {
        public NetWinLoss(decimal winloss, decimal comm)
        {
            WinLoss = winloss;
            Comm = comm;
            Total = winloss + comm;
        }

        public NetWinLoss(decimal total)
        {
            Total = total;
        }

        public decimal? WinLoss { get; }

        public decimal? Comm { get; }

        public decimal Total { get; }
    }
}