namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    public class BetCreditHistoryViewModel
    {
        public BetCreditHistoryViewModel(string custLevel)
        {
            CustLevel = custLevel;
        }

        public string CustLevel { get; }
    }
}