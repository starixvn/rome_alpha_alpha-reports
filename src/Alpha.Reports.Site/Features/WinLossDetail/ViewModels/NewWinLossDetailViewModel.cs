namespace Alpha.Reports.Site.Features.WinLossDetail.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.WinLossDetail.Requests;
    using Alpha.Reports.Site.ViewModels.Components;

    public class NewWinLossDetailViewModel
    {
        public NewWinLossDetailViewModel(
            NewWinLossDetailQuery message,
            IEnumerable<GroupProductViewModel> products,
            IEnumerable<ProductDetailViewModel> productDetails,
            EnteredSportResult totalEnteredSport)
        {
            Title = GetReportTitle(message.CustLevel);
            Language = message.Language;
            DateRangePickerViewModel = new DateRangePickerViewModel(
                message.MinDate,
                DateTime.Today,
                message.FromDate,
                message.ToDate,
                0);
            Products = products;
            ProductDetails = productDetails;
            TotalEnteredSport = totalEnteredSport;

        }

        public string Title { get; }

        public string Language { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public IEnumerable<GroupProductViewModel> Products { get; }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        private static string GetReportTitle(UserLevel userLevel)
            => userLevel.IsSuperLevel()
            ? Resources.MasterWinLossDetail
            : userLevel.IsMasterLevel()
                ? Resources.AgentWinLossDetail
                : Resources.MemberWinLossDetail;
    }
}