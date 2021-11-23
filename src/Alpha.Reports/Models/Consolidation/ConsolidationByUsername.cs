namespace Alpha.Reports.Models.Consolidation
{
    public class ConsolidationByUsername : ConsolidationEntity
    {
        public ConsolidationByUsername()
        {
        }

        public ConsolidationByUsername(
            int breakDown,
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
            int betCount,
            string productName,
            decimal positionTaking) : base(
                custId,
                username,
                currencyId,
                custLevel,
                turnover,
                grossComm,
                memberWinLoss,
                memberComm,
                memberTotal,
                ownerTurnover,
                ownerWinLoss,
                ownerComm,
                ownerTotal,
                company,
                betCount)
        {
            BreakDown = breakDown;
            ProductName = productName;
            PositionTaking = positionTaking;
        }

        public int BreakDown { get; }

        public string ProductName { get; }

        public decimal PositionTaking { get; }
    }
}
