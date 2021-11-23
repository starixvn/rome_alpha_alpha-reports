namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using Alpha.Reports.Models.Consolidation;
    using Alpha.Reports.Site.App_GlobalResources;

    public class ConsolidationByUsernameViewModel : ConsolidationEntityViewModel
    {
        public ConsolidationByUsernameViewModel() : base()
        {
        }

        public ConsolidationByUsernameViewModel(
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
            string currencyName = null,
            string productName = null,
            decimal positionTaking = 0) : base(
                custId,
                custLevel,
                username,
                currencyId,
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
                betCount,
                currencyName)
        {
            ProductName = productName;
            PositionTaking = positionTaking;
        }

        public string ProductName { get; }

        public decimal PositionTaking { get; }

        public string PositionTakingText
        {
            get
            {
                const int percentage = 100;
                var positionTakingPercentage = PositionTaking * percentage;

                return $"{positionTakingPercentage.ToString("#,##0.0")}% {Resources.PT}";
            }
        }

        public IEnumerable<ConsolidationByUsernameViewModel> Children { get; set; }
    }
}