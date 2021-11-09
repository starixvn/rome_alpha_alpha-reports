namespace Alpha.Reports.Models.Consolidation
{
    public class ConsolidationEntity
    {
        protected ConsolidationEntity()
        {
        }

        protected ConsolidationEntity(
            int custId,
            string username,
            byte currencyId,
            int custLevel,
            decimal turnover,
            decimal grossComm,
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
            GrossComm = grossComm;

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

        public int CustId { get; protected set; }

        public int CustLevel { get; protected set; }

        public string Username { get; protected set; }

        public byte CurrencyId { get; protected set; }

        public decimal Turnover { get; protected set; }

        public decimal GrossComm { get; protected set; }

        public decimal MemberWinLoss { get; protected set; }

        public decimal MemberComm { get; protected set; }

        public decimal MemberTotal { get; protected set; }

        public decimal OwnerTurnover { get; protected set; }

        public decimal OwnerWinLoss { get; protected set; }

        public decimal OwnerComm { get; protected set; }

        public decimal OwnerTotal { get; protected set; }

        public decimal Company { get; protected set; }

        public int BetCount { get; protected set; }
    }
}
