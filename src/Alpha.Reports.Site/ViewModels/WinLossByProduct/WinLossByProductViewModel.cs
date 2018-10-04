namespace Alpha.Reports.Site.ViewModels.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Requests.WinLossByProduct;
    using Sunplus.Agent.Model.BL.Definitions;

    public class WinLossByProductViewModel
    {
        private readonly WinLossByProductQuery message;
        private readonly WinLossByProductParam winLossByBettypeParam;

        public WinLossByProductViewModel(
            IEnumerable<WinLossByProductModel> reportData,
            WinLossByBetTypeModel grandTotal,
            WinLossByProductQuery message,
            WinLossByProductParam winLossByBettypeParam,
            Func<int, BetType, string, bool, string> buildDisplayedBettypeName,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> excludeProducts)
        {
            this.message = message;
            this.winLossByBettypeParam = winLossByBettypeParam;
            Products = products;
            ExcludeProducts = excludeProducts;
            MinDate = message.MinDate;
            ReportData = reportData;
            GrandTotal = grandTotal;

            BuildDisplayedBettypeName = buildDisplayedBettypeName;
            BuildResources();
        }

        public int CustId => message.CustId;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate { get; }

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossByBettypeParam.FromDate;

        public DateTime ToDate => winLossByBettypeParam.ToDate;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public string UserSelectedProductIds
            => winLossByBettypeParam.UserSelectedProductIds.Concat(winLossByBettypeParam.UserSelectedExcludeProductIds).JoinWithComma();

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        public IEnumerable<WinLossByProductModel> ReportData { get; }

        public WinLossByBetTypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title => Resources.WinLossByProduct;

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        private ExcludeProductViewModel ExcludeProductViewModel =>
            new ExcludeProductViewModel(ExcludeProducts, UserSelectedExcludeProductIds);

        public string UserSelectedExcludeProductIds => winLossByBettypeParam.UserSelectedExcludeProductIds.JoinWithComma();

        public WinLossByProductFilterViewModel FilterViewModel =>
            new WinLossByProductFilterViewModel(
                CustId,
                CustLevel,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel,
                ExcludeProductViewModel,
                IsFilterVisible,
                MinDate);

        private void BuildResources()
        {
            if (CustLevel.IsSuperLevel())
            {
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
            }
            else if (CustLevel.IsMasterLevel())
            {
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
            }
            else
            {
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
            }
        }

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Func<int, BetType, string, bool, string> BuildDisplayedBettypeName;
    }
}