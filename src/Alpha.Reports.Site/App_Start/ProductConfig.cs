namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using App_GlobalResources;
    using RemotingRef;
    using Sunplus.Agent.Model.Definitions;
    using ViewModels.WinLossByBetType;

    public static class ProductConfig
    {
        #region Win Loss By Bet Type

        //[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        //public static readonly IDictionary<int, Func<int, CacheRefService, WinLossGameTypeInfoViewModel>> DisplayWinLossByGameTypeRules
        //    = new Dictionary<int, Func<int, CacheRefService, WinLossGameTypeInfoViewModel>>
        //    {
        //        { ProductIdConstants.RNGCasino, RNGCasinoRule },
        //        { ProductIdConstants.NG2, WinLossGameTypeProductNameRule },
        //        { ProductIdConstants.Voidbridge, WinLossGameTypeProductNameRule }
        //    };

        //private static WinLossGameTypeInfoViewModel RNGCasinoRule(int productId, CacheRefService cacheRefService)
        //    => new WinLossGameTypeInfoViewModel(true, Resources.CasinoWinLoss);

        //private static WinLossGameTypeInfoViewModel WinLossGameTypeProductNameRule(int productId, CacheRefService cacheRefService)
        //    => new WinLossGameTypeInfoViewModel(true, $"{cacheRefService.GetProductName(productId)} {Resources.WinLossTitle}");

        #endregion Win Loss By Bet Type
    }
}