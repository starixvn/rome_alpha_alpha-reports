namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using App_GlobalResources;
    using RemotingRef;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Definitions;

    public static class BetTypeConfig
    {
        #region Win Loss By Bet Type

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly IDictionary<int, Func<BetType, string, bool, CacheRefService, string>> DisplayWinLossByBetTypeRules
           = new Dictionary<int, Func<BetType, string, bool, CacheRefService, string>>
           {
                { ProductIdConstants.RNGCasino, RNGCasinoBetTypeNameRule },
                { ProductIdConstants.LiveCasino, LiveCasinoBetTypeNameRule },
                { ProductIdConstants.VirtualSports, VirtualSportsBetTypeNameRule },
                { ProductIdConstants.GoldDeluxe, GoldDeluxeBetTypeNameRule },
                { ProductIdConstants.Voidbridge, VoidbridgeBetTypeNameRule },
                { ProductIdConstants.NG2, NG2BetTypeNameRule },
                { ProductIdConstants.AllBet, AllBetBetTypeNameRule },
                { ProductIdConstants.NumberGame, NumberGameBetTypeNameRule }
           };

        private static string RNGCasinoBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
            => betTypeName;

        private static string LiveCasinoBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
            => betTypeName;

        private static string VirtualSportsBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
            => $"{betTypeName} ({Resources.VirtualSports})";

        private static string GoldDeluxeBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
            => $"{betTypeName} ({Resources.GoldDeluxe})";

        private static string VoidbridgeBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
            => Resources.Voidbridge;

        private static string NG2BetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
         => cacheRefService.GetProductName(ProductIdConstants.NG2);

        private static string AllBetBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
        => $"{betTypeName} ({Resources.Allbet})";

        private static string NumberGameBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator, CacheRefService cacheRefService)
        {
            switch (betType)
            {
                case BetType.N_Over_Under:
                    return $"{Resources.Next} {betTypeName} ({Resources.NumberGame})";
                case BetType.N_Odd_Even:
                    string strNextCombo = liveIndicator ? Resources.Next + " " : Resources.FT + ". ";
                    return $"{strNextCombo}{betTypeName} ({Resources.NumberGame})";
                case BetType.NumberWheel:
                    string typeNumberWheel = liveIndicator ? Resources.LiveGame : Resources.PreGame;
                    return $"{betTypeName} ({typeNumberWheel})";
            }

            return betTypeName;
        }

        #endregion Win Loss By Bet Type
    }
}