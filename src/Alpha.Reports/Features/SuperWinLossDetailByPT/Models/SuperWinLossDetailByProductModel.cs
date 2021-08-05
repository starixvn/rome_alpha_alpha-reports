namespace Alpha.Reports.Features.SuperWinLossDetailByPT.Models
{
    public class SuperWinLossDetailByProductModel
    {
        public SuperWinLossDetailByProductModel()
        {
        }

        public SuperWinLossDetailByProductModel(
            int productId,
            decimal turnover,
            decimal netTurnover,
            decimal grossComm,
            decimal memberWinLost,
            decimal memberComm,
            decimal memberTotal,
            decimal agWinLost,
            decimal agComm,
            decimal agTotal,
            decimal mAgWinLost,
            decimal mAgComm,
            decimal mAgTotal,
            decimal sAgWinLost,
            decimal sAgComm,
            decimal sAgTotal,
            decimal company)
        {
            ProductId = productId;
            Turnover = turnover;
            NetTurnover = netTurnover;
            GrossComm = grossComm;

            MemberWinLost = memberWinLost;
            MemberComm = memberComm;
            MemberTotal = memberTotal;

            AgWinLost = agWinLost;
            AgComm = agComm;
            AgTotal = agTotal;

            MAgWinLost = mAgWinLost;
            MAgComm = mAgComm;
            MAgTotal = mAgTotal;

            SAgWinLost = sAgWinLost;
            SAgComm = sAgComm;
            SAgTotal = sAgTotal;

            Company = company;
        }

        public int ProductId { get; }

        public decimal Turnover { get; }

        public decimal NetTurnover { get; }

        public decimal GrossComm { get; }

        public decimal MemberWinLost { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal { get; }

        public decimal AgWinLost { get; }

        public decimal AgComm { get; }

        public decimal AgTotal { get; }

        public decimal MAgWinLost { get; }

        public decimal MAgComm { get; }

        public decimal MAgTotal { get; }

        public decimal SAgWinLost { get; }

        public decimal SAgComm { get; }

        public decimal SAgTotal { get; }

        public decimal Company { get; }
    }
}