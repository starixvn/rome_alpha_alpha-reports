namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Diagnostics.CodeAnalysis;
    using App_GlobalResources;
    using Reports.Models;
    using Reports.Models.Consolidation;

    public class ConsolidationByUsernameViewModel : ConsolidationByUsername
    {        
        public ConsolidationByUsernameViewModel(int custId, int custLevel, string username, byte currencyId, decimal turnover, decimal memberWinLoss, decimal memberComm, decimal memberTotal, decimal ownerTurnover, decimal ownerWinLoss, decimal ownerComm, decimal ownerTotal, decimal company, int betCount, string currencyName) :
            base(custId, custId, username, currencyId, custLevel, turnover, memberWinLoss, memberComm, memberTotal, ownerTurnover, ownerWinLoss, ownerComm, ownerTotal, company, betCount)
        {
            CurrencyName = currencyName;
        }

        public string CurrencyName { get; }

        public string CustLevelName
            => CustLevel == UserLevel.Super.Value
            ? Resources.Super
            : CustLevel == UserLevel.Master.Value
            ? Resources.Master
            : CustLevel == UserLevel.Agent.Value
            ? Resources.Agent
            : Resources.Member;
    }
}