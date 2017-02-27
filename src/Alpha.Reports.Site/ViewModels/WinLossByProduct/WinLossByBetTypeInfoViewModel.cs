namespace Alpha.Reports.Site.ViewModels.WinLossByProduct
{
    public class WinLossByBetTypeInfoViewModel
    {
        public WinLossByBetTypeInfoViewModel(
           bool isAllowViewBetType,
           string winLossByBetTypeTitle)
        {
            IsAllowViewBetType = isAllowViewBetType;
            WinLossByBetTypeTitle = winLossByBetTypeTitle;
        }

        public bool IsAllowViewBetType { get; }

        public string WinLossByBetTypeTitle { get; }
    }
}