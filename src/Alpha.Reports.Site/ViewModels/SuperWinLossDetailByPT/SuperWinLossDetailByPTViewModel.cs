namespace Alpha.Reports.Site.ViewModels.SuperWinLossDetailByPT
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models.SuperWinLossDetailByPT;
    using Alpha.Reports.Params.SuperWinLossDetailByPT;
    using App_GlobalResources;
    using Components;
    using Requests.SuperWinLossDetailByPT;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SuperWinLossDetailByPTViewModel
    {
        private readonly SuperWinLossDetailByPTQuery message;
        private readonly SuperWinLossDetailByPTParam param;

        public SuperWinLossDetailByPTViewModel(
            IEnumerable<SuperWinLossDetailByPTModel> reportData,
            SuperWinLossDetailModel grandTotal,
            SuperWinLossDetailByPTQuery message,
            SuperWinLossDetailByPTParam param,
            IEnumerable<SuperPtProductViewModel> ptProducts,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<ProductDetailViewModel> productDetails)
        {
            this.message = message;
            this.param = param;

            PtProducts = ptProducts;
            GroupProducts = groupProducts;
            ProductDetails = productDetails;
            ReportData = reportData;
            GrandTotal = grandTotal;
        }

        public int CustId => message.CustId;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate { get; } = DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public bool IsSetDefaultStackMode
            => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption
            => message.UserSelectedDateRangeOption;

        public string UserSelectedProductIds
            => param.UserSelectedProductIds.JoinWithComma();

        public string UserSelectedResourceIds
            => param.UserSelectedResourceIds.JoinWithComma();

        public IEnumerable<SuperPtProductViewModel> PtProducts { get; }

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public IEnumerable<SuperWinLossDetailByPTModel> ReportData { get; }

        public SuperWinLossDetailModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title { get; } = Resources.SuperWinLossDetailByPT;

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(GroupProducts, UserSelectedProductIds, Language);

        private ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel =>
            new ProductDetailMultiSelectionViewModel(ProductDetails, UserSelectedResourceIds, Language);

        public SuperWinLossDetailByPTFilterViewModel FilterViewModel =>
            new SuperWinLossDetailByPTFilterViewModel(
                CustId,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel,
                ProductDetailMultiSelectionViewModel,
                IsFilterVisible,
                MinDate);
    }
}