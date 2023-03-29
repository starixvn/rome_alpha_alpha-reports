namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Reports.Utils;

    public class WinLossDetailIndexViewModel
    {
        public WinLossDetailIndexViewModel(
            WinLossDetailIndexQuery message,
            IEnumerable<ProductViewModel> products,
            IEnumerable<ProductDetailNewViewModel> productDetails,
            EnteredSportResult totalEnteredSport)
        {
            ViewerId = message.ViewerId;
            ViewerName = message.CustName;
            ViewerLevel = message.ViewerLevel;
            SubAccId = message.SubAccId;
            CanViewBetLists = !message.IsProtectedMode;
            DateRangePickerViewModel = new DateRangePickerNewViewModel(
                message.FromDate,
                message.ToDate,
                message.MinDate,
                message.HistoricMinDate);
            Products = products;
            ProductDetails = productDetails;
            TotalEnteredSport = totalEnteredSport;
        }

        public int ViewerId { get; }

        public string ViewerName { get; }

        public UserLevel ViewerLevel { get; }

        public int SubAccId { get; }

        public bool CanViewBetLists { get; }

        public string BetListsUrl => Helpers.UrlHelper.MapBetListPath("~/WinLoss/Index");

        public DateRangePickerNewViewModel DateRangePickerViewModel { get; }

        public IEnumerable<ProductViewModel> Products { get; }

        public IEnumerable<ProductDetailNewViewModel> ProductDetails { get; }

        public IEnumerable<SelectListItem> Columns
        {
            get
            {
                var columnIndex = 1; // Start at First/Last Name
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = Resources.FirstLastName };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = Resources.BetCount };

                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = Resources.Turnover };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = Resources.NetTurnover };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = Resources.GrossComm };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Member} {Resources.WinLoss}" };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Member} {Resources.Comm}" };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Member} {Resources.Total}" };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Agent} {Resources.WinLoss}" };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Agent} {Resources.Comm}" };
                yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Agent} {Resources.Total}" };

                if (ViewerLevel.IsMasterOrSuperLevel())
                {
                    yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Master} {Resources.WinLoss}" };
                    yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Master} {Resources.Comm}" };
                    yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Master} {Resources.Total}" };
                }

                if (ViewerLevel.IsSuperLevel())
                {
                    yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Super} {Resources.WinLoss}" };
                    yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Super} {Resources.Comm}" };
                    yield return new SelectListItem { Value = columnIndex++.ToString(), Text = $"{Resources.Super} {Resources.Total}" };
                }

                yield return new SelectListItem { Value = columnIndex.ToString(), Text = Resources.Company };
            }
        }

        public EnteredSportResult TotalEnteredSport { get; }

        public bool IsHistoricReport { get; }

        public string Title
            => ViewerLevel.IsSuperLevel()
            ? Resources.MasterWinLossDetail
            : ViewerLevel.IsMasterLevel()
                ? Resources.AgentWinLossDetail
                : Resources.MemberWinLossDetail;
    }
}