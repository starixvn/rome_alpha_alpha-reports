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
        public string BuildDisplayedBetTypeName(BetType betTypeId, string betTypeName, bool liveIndicator)
        {
            string displayedBetTypeName;

            switch (betTypeId)
            {
                case BetType.VS_1X2:
                case BetType.VS_CorrectScore:
                case BetType.VS_DoubleChange:
                case BetType.VS_Handicap:
                case BetType.VS_OverUnder:
                case BetType.VS_TotalGoal:
                case BetType.VHR_Win:
                case BetType.VHR_Place:
                case BetType.VHR_WinPlace:
                case BetType.VHR_Forecast:
                case BetType.VHR_Tricast:
                case BetType.VT_PlayerWin:
                case BetType.VT_ScoreBet:
                case BetType.VT_TotalPoints:
                    displayedBetTypeName = $"{betTypeName} ({Resources.VirtualSports})";
                    break;
                case BetType.Casino_Games:
                case BetType.Baccarat:
                case BetType.Roulette:
                case BetType.Sicbo:
                    displayedBetTypeName = betTypeName;
                    break;
                case BetType.GoldDeluxeBaccarat:
                case BetType.GoldDeluxeRoulette:
                case BetType.GoldDeluxeParlayBaccarat:
                    displayedBetTypeName = $"{betTypeName} ({Resources.GoldDeluxe})";
                    break;
                case BetType.N_Over_Under:
                    displayedBetTypeName = $"{Resources.Next} {betTypeName} ({Resources.NumberGame})";
                    break;
                case BetType.N_Odd_Even:
                    string strNextCombo = liveIndicator ? Resources.Next + " " : Resources.FT + ". ";
                    displayedBetTypeName = $"{strNextCombo}{betTypeName} ({Resources.NumberGame})";
                    break;
                case BetType.NumberWheel:
                    string typeNumberWheel = liveIndicator ? Resources.LiveGame : Resources.PreGame;
                    displayedBetTypeName = $"{betTypeName} ({typeNumberWheel})";
                    break;            
                case BetType.VoidbridgeRangeStart:
                    displayedBetTypeName = Resources.Voidbridge;
                    break;
                case BetType.NG2Representative:
                    displayedBetTypeName = cacheRefService.GetProductName(ProductIdConstants.NG2);
                    break;
                default:
                    displayedBetTypeName = betTypeName;
                    if (betTypeId >= BetType.AllBetRangeStart && betTypeId <= BetType.AllBetRangeEnd)
                    {
                        displayedBetTypeName = $"{betTypeName} ({Resources.Allbet})";
                    }

                    break;
            }

            return displayedBetTypeName;
        }
    }
}