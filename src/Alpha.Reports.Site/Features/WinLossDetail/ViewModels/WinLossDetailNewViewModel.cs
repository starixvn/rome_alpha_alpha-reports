namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Web.Helpers;

    public class WinLossDetailNewViewModel : WinLossDetailGetDataViewModel
    {
        public WinLossDetailNewViewModel(
            WinLossDetailNewQuery message,
            IEnumerable<ProductViewModel> products,
            IEnumerable<ProductDetailNewViewModel> productDetails,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail,
            IEnumerable<WeekSelectionViewModel> weeks)
            : base(message, totalEnteredSport, winLossDetail, weeks)
        {
            ViewerId = message.ViewerId;
            ViewerName = message.CustName;
            ViewerLevel = message.ViewerLevel;
            SubAccId = message.SubAccId;
            CustId = message.CustId;
            CustName = message.CustName;
            DateRangePickerViewModel = new DateRangePickerNewViewModel(
                message.FromDate,
                message.ToDate,
                message.MinDate);
            Products = products;
            ProductIds = message.ProductIds;
            ProductDetails = productDetails;
            ResourceIds = message.ResourceIds;
            Username = message.Username;
            WeekFromDate = message.WeekFromDate.HasValue ? message.WeekFromDate.Value.ToShortDateWithSlash() : string.Empty;
            WeekToDate = message.WeekToDate.HasValue ? message.WeekToDate.Value.ToShortDateWithSlash() : string.Empty;
            Language = message.Language;
            MainDomain = message.IsInternal ? message.MainDomain : string.Empty;
            ShowColumns = message.ShowColumns;
        }

        public int ViewerId { get; }

        public string ViewerName { get; }

        public UserLevel ViewerLevel { get; }

        public int SubAccId { get; }

        public int CustId { get; }

        public string CustName { get; }

        public string BetListsUrl => Helpers.UrlHelper.MapBetListPath("~/WinLoss/IndexNew");

        public string UsernameAutoSuggestionUrl => MicroSites.MemberInfoUrl("~/UserNameSuggestion/GetUsernamesSuggestion");

        public DateRangePickerNewViewModel DateRangePickerViewModel { get; }

        public IEnumerable<ProductViewModel> Products { get; }

        public string ProductIds { get; }

        public IEnumerable<ProductDetailNewViewModel> ProductDetails { get; }

        public string ResourceIds { get; }

        public string Username { get; }

        public string WeekFromDate { get; }

        public string WeekToDate { get; }

        public string Language { get; }

        public string MainDomain { get; }

        public string ShowColumns { get; }
    }
}