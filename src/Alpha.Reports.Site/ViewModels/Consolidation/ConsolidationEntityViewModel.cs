namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using App_GlobalResources;
    using Reports.Models;
    using Reports.Models.Consolidation;

    public class ConsolidationEntityViewModel : ConsolidationEntity
    {
        public ConsolidationEntityViewModel() : base()
        {
        }

        public ConsolidationEntityViewModel(
            int custId,
            int custLevel,
            string username,
            byte currencyId,
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
            string currencyName) : base(
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