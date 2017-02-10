namespace Alpha.Reports.Site.ViewModels.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using App_GlobalResources;
    using Components;
    using Helpers;
    using Params;
    using RemotingRef;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Requests.WinLossByBetType;
    using Sunplus.Agent.Model.BL.Definitions;
    using Web.Helpers;

    public class WinLossByBetTypeViewModel
    {
        private readonly string sortPropertyNameDefault = "BetTypeName";    
        private readonly string winLossByGameTypePath = "WinLossByGameType/Index";

        private readonly WinLossByBetTypeQuery message;
        private readonly WinLossByBetTypeParam winLossByBettypeParam;
        private readonly CacheRefService cacheRefService;

        public WinLossByBetTypeViewModel(
            IEnumerable<WinLossByBetTypeModel> reportData,
            WinLossByBetTypeModel grandTotal,
            WinLossByBetTypeQuery message,
            WinLossByBetTypeParam winLossByBettypeParam,
            Func<int, string, bool, string> buildDisplayedBettypeName,
            CacheRefService cacheRefService)
        {
            this.message = message;
            this.winLossByBettypeParam = winLossByBettypeParam;
            MinDate = message.MinDate;
            ReportData = reportData;
            GrandTotal = grandTotal;
            this.cacheRefService = cacheRefService;

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

        public IEnumerable<WinLossByBetTypeModel> ReportData { get; }

        public WinLossByBetTypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortedProperty(message.CustLevel, message.SortPropertyName);

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title => Resources.WinLossByBetType;

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string CustLevelNetWinLossName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public WinLossByBetTypeFilterViewModel FilterViewModel =>
            new WinLossByBetTypeFilterViewModel(
                CustId,
                CustLevel,
                DateRangePickerViewModel,
                IsFilterVisible,
                MinDate);

        private string GetSortedProperty(UserLevel custLevel, string sortedProperty)
        {
            if (string.IsNullOrEmpty(sortedProperty))
            {
                return sortPropertyNameDefault;
            }

            var isSortedPropertyInRange =
                custLevel.IsSuperLevel()
                || (custLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (custLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            return isSortedPropertyInRange ? sortedProperty : sortPropertyNameDefault;
        }

        ///// <summary>
        ///// TODO refactor Title
        ///// </summary>
        private void BuildResources()
        {
            if (CustLevel.IsSuperLevel())
            {
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
                CustLevelNetWinLossName = "SuperNetWinLoss";
            }
            else if (CustLevel.IsMasterLevel())
            {
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
                CustLevelNetWinLossName = "MasterNetWinLoss";
            }
            else
            {
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
                CustLevelNetWinLossName = "AgentNetWinLoss";
            }
        }

        public string BuildWinLossByGameTypePath(int productId, string title)
        {
            return UrlHelper.AddQueryStringParamsFunc(
                 AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/") + winLossByGameTypePath,
                 new Dictionary<string, object>
            {
                { "UserSelectedDateRangeOption", UserSelectedDateRangeOption },
                { "FromDate", FromDate },
                { "ToDate", ToDate },
                { "ProductId", productId },
                { "IsStackMode",  IsStackMode },
                { "SortPropertyName", SortPropertyName },
                { "IsDescendingSort", IsDescendingSort },
                { "IsFilterVisible", IsFilterVisible },
                { "Title", title },
                { "IsSetDefaultStackMode", IsSetDefaultStackMode }
            });
        }

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Func<int, string, bool, string> BuildDisplayedBettypeName;

        /// <summary>
        /// TODO: Multiple Language
        /// </summary>
        /// <param name="betTypeId"></param>
        /// <returns></returns>
        public WinLossGameTypeInfoViewModel BuildWinLossGameTypeInfoViewModel(int betTypeId, int productId)
        {
            bool isAllowViewGameType = false;
            string winLossByGameTypeTitle = string.Empty;

            switch (betTypeId)
            {
                case (int)BetType.Casino_Games:
                    {
                        isAllowViewGameType = true;
                        winLossByGameTypeTitle = Resources.CasinoWinLoss;
                    }

                    break;
                case (int)BetType.VoidbridgeRangeStart: //// {ProductName} + Win Loss
                    {
                        isAllowViewGameType = true;
                        winLossByGameTypeTitle = cacheRefService.GetProductName(productId) + Resources.WinLossTitle;
                    }

                    break;
                case (int)BetType.NG2Start + 1:
                    {
                        isAllowViewGameType = true;
                        winLossByGameTypeTitle = cacheRefService.GetProductName(productId) + Resources.WinLossTitle;
                    }

                    break;
                default: break;
            }

            return new WinLossGameTypeInfoViewModel(
                isAllowViewGameType: isAllowViewGameType,
                winLossByGameTypeTitle: winLossByGameTypeTitle);
        }
    }
}