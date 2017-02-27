namespace Alpha.Reports.Site.ViewModels.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using App_GlobalResources;
    using App_Start;
    using Components;
    using Extensions;
    using Params;
    using RemotingRef;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Requests.WinLossByBetType;
    using Sunplus.Agent.Model.BL.Definitions;

    public class WinLossByBetTypeViewModel
    {
        private readonly WinLossByBetTypeQuery message;
        private readonly WinLossByBetTypeParam winLossByBettypeParam;
        private readonly CacheRefService cacheRefService;
        public static readonly string NetWinLossWinLoss = $"{nameof(WinLossByBetTypeModel.AgentNetWinLoss)}.{nameof(NetWinLoss.WinLoss)}";
        public static readonly string NetWinLossComm = $"{nameof(WinLossByBetTypeModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Comm)}";
        public static readonly string NetWinLossTotal = $"{nameof(WinLossByBetTypeModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Total)}";

        public WinLossByBetTypeViewModel(
            IEnumerable<WinLossByBetTypeModel> reportData,
            WinLossByBetTypeModel grandTotal,
            WinLossByBetTypeQuery message,
            WinLossByBetTypeParam winLossByBettypeParam,
            Func<int, BetType, string, bool, string> buildDisplayedBettypeName,
            IEnumerable<SelectListItem> products,
            CacheRefService cacheRefService)
        {
            this.message = message;
            this.winLossByBettypeParam = winLossByBettypeParam;
            this.cacheRefService = cacheRefService;
            Products = products;
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

        public string UserSelectedProductIds => winLossByBettypeParam.UserSelectedProductIds.JoinWithComma();

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<WinLossByBetTypeModel> ReportData { get; }

        public WinLossByBetTypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title => Resources.WinLossByBetType;

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossByBetTypeFilterViewModel FilterViewModel =>
            new WinLossByBetTypeFilterViewModel(
                CustId,
                CustLevel,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel,
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

        public string BuildWinLossByGameTypePath(int productId, string title, string winLossByGameTypePath)
        {
            return Helpers.UrlHelper.AddQueryStringParamsFunc(
                 winLossByGameTypePath,
                 new Dictionary<string, object>
            {
                { "UserSelectedDateRangeOption", UserSelectedDateRangeOption },
                { "FromDate", FromDate },
                { "ToDate", ToDate },
                { "ProductId", productId },
                { "IsStackMode",  IsStackMode },
                { "IsDescendingSort", IsDescendingSort },
                { "IsFilterVisible", IsFilterVisible },
                { "Title", title },
                { "IsSetDefaultStackMode", IsSetDefaultStackMode }
            });
        }

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Func<int, BetType, string, bool, string> BuildDisplayedBettypeName;

        public WinLossGameTypeInfoViewModel BuildWinLossGameTypeInfoViewModel(int productId)
        {
            if (ProductConfig.DisplayWinLossByGameTypeRules.ContainsKey(productId))
            {
                return ProductConfig.DisplayWinLossByGameTypeRules[productId](productId, cacheRefService);
            }

            return new WinLossGameTypeInfoViewModel(false, string.Empty);
        }
    }
}