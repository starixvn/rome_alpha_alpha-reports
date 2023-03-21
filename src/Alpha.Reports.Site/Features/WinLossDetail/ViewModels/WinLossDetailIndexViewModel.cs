namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Reports.Utils;

    public class WinLossDetailIndexViewModel
    {
        public WinLossDetailIndexViewModel(
            WinLossDetailIndexQuery message,
            IEnumerable<GroupProductViewModel> products,
            IEnumerable<ProductDetailViewModel> productDetails,
            EnteredSportResult totalEnteredSport)
        {
            CustId = message.CustId;
            CustLevel = message.CustLevel;
            DateRangePickerViewModel = new DateRangePickerViewModelNew(message.MinDate, SystemTime.Today, message.FromDate, message.ToDate);
            Products = products;
            ProductDetails = productDetails;
            TotalEnteredSport = totalEnteredSport;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModelNew DateRangePickerViewModel { get; }

        public IEnumerable<GroupProductViewModel> Products { get; }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        public string Title
            => CustLevel.IsSuperLevel()
            ? Resources.MasterWinLossDetail
            : CustLevel.IsMasterLevel()
                ? Resources.AgentWinLossDetail
                : Resources.MemberWinLossDetail;
    }
}