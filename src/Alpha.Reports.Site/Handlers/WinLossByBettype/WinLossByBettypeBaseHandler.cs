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
        public string BuildDisplayedBetTypeName(int productId, BetType betTypeId, string betTypeName, bool liveIndicator)
        {
            string displayedBetTypeName = betTypeName;

            switch (productId)
            {
                case ProductIdConstants.RNGCasino:
                case ProductIdConstants.LiveCasino:
                    break;
                case ProductIdConstants.VirtualSports:
                    displayedBetTypeName = $"{betTypeName} ({Resources.VirtualSports})";
                    break;
                case ProductIdConstants.GoldDeluxe:
                    displayedBetTypeName = $"{betTypeName} ({Resources.GoldDeluxe})";
                    break;
                case ProductIdConstants.NumberGame:
                    {
                        switch (betTypeId)
                        {
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
                        }
                    }

                    break;
                case ProductIdConstants.Voidbridge:
                    displayedBetTypeName = Resources.Voidbridge;
                    break;
                case ProductIdConstants.NG2:
                    displayedBetTypeName = cacheRefService.GetProductName(ProductIdConstants.NG2);
                    break;
                case ProductIdConstants.AllBet:
                    displayedBetTypeName = $"{betTypeName} ({Resources.Allbet})";
                    break;
            }

            return displayedBetTypeName;
        }
    }
}