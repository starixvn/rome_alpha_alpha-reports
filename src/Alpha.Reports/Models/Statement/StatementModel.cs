namespace Alpha.Reports.Models.Statement
{
    using System;
    using Alpha.Reports.Extensions;

    public class StatementModel
    {
        public StatementModel()
        {
        }

        public StatementModel(
            DateTime winLossDate,
            int custId,
            decimal? openingBalance,
            decimal? voidAmount,
            decimal? outstanding,
            decimal? turnover,
            decimal? transferAmount,
            decimal? winLoss,
            decimal? commission,
            decimal? closingBalance,
            decimal? grandPrize)
        {
            WinLossDate = winLossDate;
            CustId = custId;
            OpeningBalance = openingBalance;
            Turnover = turnover;
            WinLoss = winLoss;
            Commission = commission;
            TransferAmount = transferAmount;
            ClosingBalance = closingBalance;
            Outstanding = outstanding;
            VoidAmount = voidAmount;
            GrandPrize = grandPrize;
        }

        public DateTime WinLossDate { get; set; }

        public int CustId { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? Turnover { get; set; }

        public decimal? WinLoss { get; set; }

        public decimal? Commission { get; set; }

        public decimal? TransferAmount { get; set; }

        public decimal? ClosingBalance { get; set; }

        public decimal? Outstanding { get; set; }

        public decimal? VoidAmount { get; set; }

        public decimal? GrandPrize { get; set; }

        public string WinLossDateString => WinLossDate.ToShortDateWithSlash();

        public bool IsNotedBalance(StatementModel previousStatement)
            => previousStatement != null
            && previousStatement.ClosingBalance != OpeningBalance;
    }
}
