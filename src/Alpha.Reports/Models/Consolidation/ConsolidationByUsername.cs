namespace Alpha.Reports.Models.Consolidation
{
    public class ConsolidationByUsername
    {
        public ConsolidationByUsername(
            string breakDown,
            int custId,
            string username,
            byte currencyId,
            int custLevel,
            decimal turnover,
            decimal memberWinLoss,
            decimal memberComm,
            decimal memberTotal,
            decimal ownerTurnover,
            decimal ownerWinLoss,
            decimal ownerComm,
            decimal ownerTotal,
            decimal company,
            int betCount)
        {
            CustId = custId;
            CustLevel = custLevel;
            Username = username;
            CurrencyId = currencyId;
            Turnover = turnover;

            MemberWinLoss = memberWinLoss;
            MemberComm = memberComm;
            MemberTotal = memberTotal;
            OwnerTurnover = ownerTurnover;
            OwnerWinLoss = ownerWinLoss;
            OwnerComm = ownerComm;
            OwnerTotal = ownerTotal;

            Company = company;
            BetCount = betCount;
        }

        public int CustId { get; }

        public int CustLevel { get; }

        public string Username { get; }

        public byte CurrencyId { get; }

        public decimal Turnover { get; }

        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal { get; }

        public decimal OwnerTurnover { get; }

        public decimal OwnerWinLoss { get; }

        public decimal OwnerComm { get; }

        public decimal OwnerTotal { get; }

        public decimal Company { get; }

        public int BetCount { get; }
    }
}
