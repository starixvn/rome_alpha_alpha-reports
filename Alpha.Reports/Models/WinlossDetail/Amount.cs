namespace Alpha.Reports.Models.WinlossDetail
{
    public class Amount
    {
        public Amount(decimal winloss, decimal comm)
        {
            WinLoss = winloss;
            Comm = comm;
        }

        public decimal WinLoss { get; }

        public decimal Comm { get; }

        public decimal Value
        {
            get { return WinLoss + Comm; }
        }
    }
}
