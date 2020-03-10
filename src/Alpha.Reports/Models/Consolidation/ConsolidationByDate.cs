namespace Alpha.Reports.Models.Consolidation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;

    public class ConsolidationByDate : ConsolidationByUsername
    {
        public ConsolidationByDate()
        {
        }
        
        public ConsolidationByDate(string breakDown, int custId, string username, byte currencyId, int custLevel, decimal turnover, decimal grossComm, decimal memberWinLoss, decimal memberComm, decimal memberTotal, decimal ownerTurnover, decimal ownerWinLoss, decimal ownerComm, decimal ownerTotal, decimal company, int betCount) :
            base(custId, custId, username, currencyId, custLevel, turnover, grossComm, memberWinLoss, memberComm, memberTotal, ownerTurnover, ownerWinLoss, ownerComm, ownerTotal, company, betCount)
        {
            BreakDown = breakDown;
        }

        public DateTime Date => Converter.ToDateTime(BreakDown);

        public string BreakDown { get; }
    }
}
