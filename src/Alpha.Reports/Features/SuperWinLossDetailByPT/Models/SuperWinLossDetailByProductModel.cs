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

        public int ProductId { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinLost { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgWinLost { get; set; }

        public decimal AgComm { get; set; }

        public decimal AgTotal { get; set; }

        public decimal MAgWinLost { get; set; }

        public decimal MAgComm { get; set; }

        public decimal MAgTotal { get; set; }

        public decimal SAgWinLost { get; set; }

        public decimal SAgComm { get; set; }

        public decimal SAgTotal { get; set; }

        public decimal Company { get; set; }
    }
}