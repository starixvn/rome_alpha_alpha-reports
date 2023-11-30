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

        public int CustId { get; set; }

        public int CustLevel { get; set; }

        public string Username { get; set; }

        public byte CurrencyId { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal OwnerTurnover { get; set; }

        public decimal OwnerWinLoss { get; set; }

        public decimal OwnerComm { get; set; }

        public decimal OwnerTotal { get; set; }

        public decimal Company { get; set; }

        public int BetCount { get; set; }
    }
}
