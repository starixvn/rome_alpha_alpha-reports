namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailNewViewModel
    {
        public WinLossDetailNewViewModel(
            WinLossDetailNewQuery message,
            IEnumerable<ProductViewModel> products,
            IEnumerable<ProductDetailNewViewModel> productDetails,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail)
        {
            ViewerId = message.ViewerId;
            ViewerName = message.CustName;
            ViewerLevel = message.ViewerLevel;
            SubAccId = message.SubAccId;
            CustId = message.CustId;
            CustName = message.CustName;
            CustLevel = message.CustLevel;
            CanViewBetLists = !message.IsProtectedMode;
            DateRangePickerViewModel = new DateRangePickerNewViewModel(
                message.FromDate,
                message.ToDate,
                message.HistoricPivotDate,
                message.MinDate);
            Products = products;
            ProductIds = message.ProductIds;
            ProductDetails = productDetails;
            ResourceIds = message.ResourceIds;
            TotalEnteredSport = totalEnteredSport;
            WinLossDetail = winLossDetail;
            BreadcrumbLevelJson = message.BreadcrumbLevelJson;
        }

        public int ViewerId { get; }

        public string ViewerName { get; }

        public UserLevel ViewerLevel { get; }

        public int SubAccId { get; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public bool CanViewBetLists { get; }

        public string BetListsUrl => Helpers.UrlHelper.MapBetListPath("~/WinLoss/IndexNew");

        public DateRangePickerNewViewModel DateRangePickerViewModel { get; }

        public IEnumerable<ProductViewModel> Products { get; }

        public string ProductIds { get; }

        public IEnumerable<ProductDetailNewViewModel> ProductDetails { get; }

        public string ResourceIds { get; }

        public IEnumerable<SelectListItem> Columns
        {
            get
            {
                yield return new SelectListItem { Value = "fullname", Text = Resources.FirstLastName };
                yield return new SelectListItem { Value = "betcount", Text = Resources.BetCount };
                yield return new SelectListItem { Value = "turnover", Text = Resources.Turnover };
                yield return new SelectListItem { Value = "netturnover", Text = Resources.NetTurnover };
                yield return new SelectListItem { Value = "grosscomm", Text = Resources.GrossComm };
                yield return new SelectListItem { Value = "pwinloss", Text = $"{Resources.Member} {Resources.WinLoss}" };
                yield return new SelectListItem { Value = "pcomm", Text = $"{Resources.Member} {Resources.Comm}" };
                yield return new SelectListItem { Value = "ptotal", Text = $"{Resources.Member} {Resources.Total}" };
                yield return new SelectListItem { Value = "awinloss", Text = $"{Resources.Agent} {Resources.WinLoss}" };
                yield return new SelectListItem { Value = "acomm", Text = $"{Resources.Agent} {Resources.Comm}" };
                yield return new SelectListItem { Value = "atotal", Text = $"{ Resources.Agent} {Resources.Total}" };

                if (ViewerLevel.IsMasterOrSuperLevel())
                {
                    yield return new SelectListItem { Value = "mwinloss", Text = $"{Resources.Master} {Resources.WinLoss}" };
                    yield return new SelectListItem { Value = "mcomm", Text = $"{Resources.Master} {Resources.Comm}" };
                    yield return new SelectListItem { Value = "mtotal", Text = $"{Resources.Master} {Resources.Total}" };
                }

                if (ViewerLevel.IsSuperLevel())
                {
                    yield return new SelectListItem { Value = "swinloss", Text = $"{Resources.Super} {Resources.WinLoss}" };
                    yield return new SelectListItem { Value = "scomm", Text = $"{Resources.Super} {Resources.Comm}" };
                    yield return new SelectListItem { Value = "stotal", Text = $"{Resources.Super} {Resources.Total}" };
                }

                yield return new SelectListItem { Value = "company", Text = Resources.Company };
            }
        }

        public EnteredSportResult TotalEnteredSport { get; }

        public PagedReportData<WinLossDetailModel> WinLossDetail { get; }

        public string BreadcrumbLevelJson { get; }
    }
}