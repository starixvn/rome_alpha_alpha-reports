namespace Alpha.Reports.Site.ViewModels.WinLossByBettype
{
    public class BetTypeDetailViewModel
    {
        public BetTypeDetailViewModel(
           string betTypeNameViewer,
           bool isAllowViewGameType,
           string winLossByGameTypeTitle)
        {
            BetTypeNameViewer = betTypeNameViewer;
            IsAllowViewGameType = isAllowViewGameType;
            WinLossByGameTypeTitle = winLossByGameTypeTitle;
        }

        public string BetTypeNameViewer { get; }

        public bool IsAllowViewGameType { get; }

        public string WinLossByGameTypeTitle { get; }
    }
}