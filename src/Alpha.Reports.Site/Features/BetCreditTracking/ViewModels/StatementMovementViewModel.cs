namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    public class StatementMovementViewModel
    {
        public StatementMovementViewModel(string custLevel)
        {
            CustLevel = custLevel;
        }

        public string CustLevel { get; }
    }
}