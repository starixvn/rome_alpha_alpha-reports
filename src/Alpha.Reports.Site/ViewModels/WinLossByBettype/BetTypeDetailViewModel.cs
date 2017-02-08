namespace Alpha.Reports.Site.ViewModels.WinLossByBetType
{
    public class BetTypeDetailViewModel
    {
        public BetTypeDetailViewModel(
           string betTypeNameViewer,
           bool isAllowViewGameType,
           string winLossByGameTypeTitle)
        {
            BetTypeNameDisplay = betTypeNameViewer;
            IsAllowViewGameType = isAllowViewGameType;
            WinLossByGameTypeTitle = winLossByGameTypeTitle;
        }

        public string BetTypeNameDisplay { get; }

        public bool IsAllowViewGameType { get; }

        public string WinLossByGameTypeTitle { get; }
    }
}