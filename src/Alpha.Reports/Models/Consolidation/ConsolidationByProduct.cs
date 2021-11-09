namespace Alpha.Reports.Models.Consolidation
{
    using Fanex.Common;

    public class ConsolidationByProduct : ConsolidationEntity
    {
        public ConsolidationByProduct()
        {
        }

        public ConsolidationByProduct(
            byte breakDown,
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
            int betCount) : base(
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
        }

        public int Product => Converter.ToInt(BreakDown);

        public byte BreakDown { get; }
    }
}
