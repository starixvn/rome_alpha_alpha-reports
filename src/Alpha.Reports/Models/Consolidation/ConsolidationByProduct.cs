namespace Alpha.Reports.Models.Consolidation
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;

    public class ConsolidationByProduct : ConsolidationByUsername
    {
        public ConsolidationByProduct()
        {

        }

        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2100:CodeLineMustNotBeLongerThan",
            Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public ConsolidationByProduct(byte breakDown, int custId, string username, byte currencyId, int custLevel, decimal turnover, decimal memberWinLoss, decimal memberComm, decimal memberTotal, decimal ownerTurnover, decimal ownerWinLoss, decimal ownerComm, decimal ownerTotal, decimal company, int betCount) :
            base(custId, custId, username, currencyId, custLevel, turnover, memberWinLoss, memberComm, memberTotal, ownerTurnover, ownerWinLoss, ownerComm, ownerTotal, company, betCount)
        {
            Product = Converter.ToInt(breakDown);
        }

        public int Product { get; }
    }
}
