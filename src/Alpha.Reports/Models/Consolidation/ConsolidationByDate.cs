namespace Alpha.Reports.Models.Consolidation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;

    public class ConsolidationByDate : ConsolidationByUsername
    {
        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2100:CodeLineMustNotBeLongerThan",
            Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public ConsolidationByDate(string breakDown, int custId, string username, byte currencyId, int custLevel, decimal turnover, decimal memberWinLoss, decimal memberComm, decimal memberTotal, decimal ownerTurnover, decimal ownerWinLoss, decimal ownerComm, decimal ownerTotal, decimal company, int betCount) :
            base(breakDown, custId, username, currencyId, custLevel, turnover, memberWinLoss, memberComm, memberTotal, ownerTurnover, ownerWinLoss, ownerComm, ownerTotal, company, betCount)
        {
            Date = Converter.ToDateTime(breakDown);
        }

        public DateTime Date { get; }
    }
}
