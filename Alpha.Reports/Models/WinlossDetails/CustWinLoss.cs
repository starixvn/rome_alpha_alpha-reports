namespace Alpha.Reports.Models.WinlossDetails
{
    public class CustWinLoss
    {
        public CustWinLoss(decimal winloss, decimal comm)
        {
            WinLoss = winloss;
            Comm = comm;
        }

        public decimal WinLoss { get; }

        public decimal Comm { get; }

        public decimal Total => WinLoss + Comm;
    }
}