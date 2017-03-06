namespace Alpha.Reports.Site.ViewModels.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using App_GlobalResources;
    using App_Start;
    using Components;
    using Helpers;
    using Params;
    using RemotingRef;
    using Reports.Models;
    using Reports.Models.WinLossByProduct;
    using Requests.WinLossByProduct;
    using Sunplus.Agent.Model.BL.Definitions;

    public class WinLossByProductViewModel
    {
        private readonly WinLossByProductQuery message;
        private readonly WinLossByProductParam winLossByProductParam;
        private readonly CacheRefService cacheRefService;
        public static readonly string NetWinLossWinLoss = $"{nameof(WinLossByProductModel.AgentNetWinLoss)}.{nameof(NetWinLoss.WinLoss)}";
        public static readonly string NetWinLossComm = $"{nameof(WinLossByProductModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Comm)}";
        public static readonly string NetWinLossTotal = $"{nameof(WinLossByProductModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Total)}";

        public WinLossByProductViewModel(
            IEnumerable<WinLossByProductModel> reportData,
            WinLossByProductModel grandTotal,
            WinLossByProductQuery message,
            WinLossByProductParam winLossByProductParam,
            Func<int, BetType, string, bool, string> buildDisplayedBettypeName,
            CacheRefService cacheRefService)
        {
            this.message = message;
            this.winLossByProductParam = winLossByProductParam;
            this.cacheRefService = cacheRefService;
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

        public DateTime FromDate => winLossByProductParam.FromDate;

        public DateTime ToDate => winLossByProductParam.ToDate;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossByProductModel> ReportData { get; }

        public WinLossByProductModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title => Resources.WinLossByProduct;

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public WinLossByProductFilterViewModel FilterViewModel =>
            new WinLossByProductFilterViewModel(
                CustId,
                CustLevel,
                DateRangePickerViewModel,
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

        public string BuildWinLossByBetTypePath(int productId, string title, string winLossByBetTypePath)
        {
            return UrlHelper.AddQueryStringParamsFunc(
                 winLossByBetTypePath,
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

        public WinLossByBetTypeInfoViewModel BuildWinLossByBetTypeInfoViewModel(int productId)
        {
            ////if (ProductConfig.DisplayWinLossByGameTypeRules.ContainsKey(productId))
            ////{
            ////    ////return ProductConfig.DisplayWinLossByGameTypeRules[productId](productId, cacheRefService);
            ////}

            return new WinLossByBetTypeInfoViewModel(false, string.Empty);
        }
    }
}