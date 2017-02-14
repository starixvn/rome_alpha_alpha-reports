namespace Alpha.Reports.Site.ViewModels.WinLossByBetType
{
    public class WinLossGameTypeInfoViewModel
    {
        public WinLossGameTypeInfoViewModel(
           bool isAllowViewGameType,
           string winLossByGameTypeTitle)
        {
            IsAllowViewGameType = isAllowViewGameType;
            WinLossByGameTypeTitle = winLossByGameTypeTitle;
        }

        public bool IsAllowViewGameType { get; }

        public string WinLossByGameTypeTitle { get; }
    }
}