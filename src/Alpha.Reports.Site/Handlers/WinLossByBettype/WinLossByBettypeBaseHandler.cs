namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Params;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Requests.WinLossByBetType;
    using Services;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Definitions;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByBetTypeBaseHandler : BaseHandler
    {
        protected readonly WinLossByBetTypeService reportService;
        protected readonly CacheRefService cacheRefService;

        public WinLossByBetTypeBaseHandler(WinLossByBetTypeService reportService, CacheRefService cacheRefService)
        {
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected WinLossByBetTypeParam CreateWinLossByBettypeParamater(WinLossByBetTypeQuery message)
        {
            return new WinLossByBetTypeParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 message.CustLevel);
        }

        internal WinLossByBetTypeModel AccumulateGrandTotal(IEnumerable<WinLossByBetTypeModel> reportData)
        {
            var grandTotal = new WinLossByBetTypeModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var data in reportData)
                {
                    grandTotal.Add(data);
                }
            }

            return grandTotal;
        }

        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules", 
            "SP2101:MethodMustNotContainMoreLinesThan", 
            Justification = "Reviewed. Suppression is OK here.")]
        public string BuildDisplayedBetTypeName(int betTypeId, string betTypeName, bool liveIndicator)
        {
            string displayedBetTypeName = string.Empty;

            switch (betTypeId)
            {
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
                    displayedBetTypeName = $"{betTypeName} ({Resources.VirtualSports})";
                    break;
                case (int)BetType.Casino_Games:
                case (int)BetType.Baccarat:
                case (int)BetType.Roulette:
                case (int)BetType.Sicbo:
                    displayedBetTypeName = betTypeName;
                    break;
                case (int)BetType.GoldDeluxeBaccarat:
                case (int)BetType.GoldDeluxeRoulette:
                case (int)BetType.GoldDeluxeParlayBaccarat:
                    displayedBetTypeName = $"{betTypeName} ({Resources.GoldDeluxe})";
                    break;
                case (int)BetType.N_Over_Under:
                    displayedBetTypeName = $"{Resources.Next} {betTypeName} ({Resources.NumberGame})";
                    break;
                case (int)BetType.N_Odd_Even:
                    string strNextCombo = liveIndicator ? Resources.Next + " " : Resources.FT + ". ";
                    displayedBetTypeName = $"{strNextCombo}{betTypeName} ({Resources.NumberGame})";
                    break;
                case (int)BetType.NumberWheel:
                    string typeNumberWheel = liveIndicator ? Resources.LiveGame : Resources.PreGame;
                    displayedBetTypeName = $"{betTypeName} ({typeNumberWheel})";
                    break;            
                case (int)BetType.VoidbridgeRangeStart:
                    displayedBetTypeName = Resources.Voidbridge;
                    break;
                case (int)BetType.NG2Start + 1:
                    displayedBetTypeName = cacheRefService.GetProductName(ProductIdConstants.NG2);
                    break;
                default:
                    displayedBetTypeName = betTypeName;
                    if (betTypeId >= (int)BetType.AllBetRangeStart && betTypeId <= (int)BetType.AllBetRangeEnd)
                    {
                        displayedBetTypeName = displayedBetTypeName = $"{betTypeName} ({Resources.Allbet})";
                    }

                    break;
            }

            return displayedBetTypeName;
        }
    }
}