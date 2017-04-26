namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using App_GlobalResources;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Definitions;

    public static class BetTypeConfig
    {
        #region Win Loss By Bet Type

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly IDictionary<int, Func<BetType, string, bool, string>> DisplayWinLossByBetTypeRules
            = new Dictionary<int, Func<BetType, string, bool, string>>
            {
                { ProductIdConstants.NumberGame, NumberGameBetTypeNameRule }
            };

        private static string NumberGameBetTypeNameRule(BetType betType, string betTypeName, bool liveIndicator)
        {
            switch (betType)
            {
                case BetType.N_Over_Under:
                    return $"{Resources.Next} {betTypeName}";
                case BetType.N_Odd_Even:
                    string strNextCombo = liveIndicator ? Resources.Next : Resources.FT;
                    return $"{strNextCombo} {betTypeName}";
                case BetType.NumberWheel:
                    string typeNumberWheel = liveIndicator ? Resources.LiveGame : Resources.PreGame;
                    return $"{betTypeName} ({typeNumberWheel})";
                default: return betTypeName;
            }
        }

        #endregion Win Loss By Bet Type
    }
}