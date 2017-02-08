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

        public WinLossByBetTypeViewModel(
            IEnumerable<WinLossByBetTypeModel> reportData,
            WinLossByBetTypeModel grandTotal,
            WinLossByBetTypeQuery message,
            WinLossByBetTypeParam winLossByBettypeParam,
            DateTime balanceUpDay)
        {
            this.message = message;
            this.winLossByBettypeParam = winLossByBettypeParam;
            MinDate = balanceUpDay;
            ReportData = reportData;
            GrandTotal = grandTotal;

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

        /// <summary>
        /// TODO : Rafactor and edit title when drill down to winloss by gametype
        /// </summary>
        /// <param name="betTypeId"></param>
        /// <param name="betTypeName"></param>
        /// <param name="liveIndicator"></param>
        /// <returns></returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public BetTypeDetailViewModel BuildBetTypeDetailViewModel(int betTypeId, string betTypeName, bool liveIndicator)
        {
            string betTypeNameViewer = string.Empty;
            bool isAllowViewGameType = false;
            string winLossByGameTypeTitle = string.Empty;

            switch (betTypeId)
            {
                case (int)BetType.Casino_Games:  //// 71
                    {
                        betTypeNameViewer = betTypeName;
                        isAllowViewGameType = true;
                        winLossByGameTypeTitle = "Casino Win Loss";
                    }

                    break;
                case (int)BetType.N_Over_Under: //// Number game Over/Under
                    betTypeNameViewer = Resources.Next + " " + betTypeName + " (" + Resources.NumberGame + ")";
                    break;
                case (int)BetType.N_Odd_Even: //// Number game Odd/Even
                    string strNextCombo = liveIndicator ? Resources.Next + " " : Resources.FT + ". ";
                    betTypeNameViewer = strNextCombo + betTypeName + " (" + Resources.NumberGame + ")";
                    break;
                case (int)BetType.NumberWheel: //// Number game - NumberWheel
                    string typeNumberWheel = liveIndicator ? Resources.LiveGame : Resources.PreGame;
                    betTypeNameViewer = betTypeName + " (" + typeNumberWheel + ")";
                    break;
                //// For Live Casino
                case (int)BetType.Baccarat: //// 1101
                case (int)BetType.Roulette: //// 1102
                case (int)BetType.Sicbo: //// 1103
                    betTypeNameViewer = betTypeName;
                    break;
                //// For Virtual Sports
                case (int)BetType.VS_1X2:
                case (int)BetType.VS_CorrectScore:
                case (int)BetType.VS_DoubleChange:
                case (int)BetType.VS_Handicap:
                case (int)BetType.VS_OverUnder:
                case (int)BetType.VS_TotalGoal:
                case (int)BetType.VHR_Win:
                case (int)BetType.VHR_Place:
                case (int)BetType.VHR_WinPlace:
                case (int)BetType.VHR_Forecast:
                case (int)BetType.VHR_Tricast:
                case (int)BetType.VT_PlayerWin:
                case (int)BetType.VT_ScoreBet:
                case (int)BetType.VT_TotalPoints:
                    betTypeNameViewer = betTypeName + " (" + Resources.VirtualSports + ")";
                    break;
                //// For Gold Deluxe
                case (int)BetType.GoldDeluxeBaccarat:
                case (int)BetType.GoldDeluxeRoulette:
                case (int)BetType.GoldDeluxeParlayBaccarat:
                    betTypeNameViewer = betTypeName + " (" + Resources.GoldDeluxe + ")";
                    break;
                case (int)BetType.VoidbridgeRangeStart:
                    {
                        betTypeNameViewer = Resources.Voidbridge;
                        isAllowViewGameType = true;
                        winLossByGameTypeTitle = "Macau Games Win Loss";
                    }

                    break;
                case (int)BetType.NG2Start + 1:
                    {
                        ////betTypeNameViewer = cacheRefService.GetProductName(ProductIdConstants.NG2);
                        betTypeNameViewer = Resources.Voidbridge;
                        isAllowViewGameType = true;
                        winLossByGameTypeTitle = "Cache Ref RG2 Win Loss";
                    }

                    break;
                default:
                    if (betTypeId >= (int)BetType.AllBetRangeStart && betTypeId <= (int)BetType.AllBetRangeEnd)
                    {
                        betTypeNameViewer = betTypeName + " (" + Resources.Allbet + ")";
                    }
                    else
                    {
                        betTypeNameViewer = betTypeName;
                    }

                    break;
            }

            return new BetTypeDetailViewModel(
                betTypeNameViewer: betTypeNameViewer,
                isAllowViewGameType: isAllowViewGameType,
                winLossByGameTypeTitle: winLossByGameTypeTitle);
        }
    }
}