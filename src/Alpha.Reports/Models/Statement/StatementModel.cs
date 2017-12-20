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

        public DateTime WinLossDate { get; }

        public int CustId { get; }

        public decimal? OpeningBalance { get; }

        public decimal? Turnover { get; }

        public decimal? WinLoss { get; }

        public decimal? Commission { get; }

        public decimal? TransferAmount { get; }

        public decimal? ClosingBalance { get; }

        public decimal? Outstanding { get; }

        public decimal? VoidAmount { get; }

        public decimal? GrandPrize { get; }

        public string WinLossDateString => WinLossDate.ToShortDateWithSlash();

        public bool IsNotedBalance(StatementModel previousStatement)
            => previousStatement != null
            && previousStatement.ClosingBalance != OpeningBalance;
    }
}
