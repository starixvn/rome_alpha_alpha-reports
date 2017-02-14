namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Site.ViewModels.WinLossByBetType;

    public static class WinLossGameTypeInfoViewModelCases
    {
        private static IEnumerable<object[]> winLossGameTypeInfoViewModel = new List<object[]>()
        {
            new object[] { 71, 22, ExpectedCasinoGameInfo },
            new object[] { 4001, 23, ExpectedMacauGameInfo },
            new object[] { 2502, 24, ExpectedNG2Info },
            new object[] { 1, 0, InValidGameTypeInfo }
        };

        private static WinLossGameTypeInfoViewModel ExpectedCasinoGameInfo =>
            new WinLossGameTypeInfoViewModel(
                isAllowViewGameType: true,
                winLossByGameTypeTitle: "Casino Win Loss");

        private static WinLossGameTypeInfoViewModel ExpectedMacauGameInfo =>
            new WinLossGameTypeInfoViewModel(
                isAllowViewGameType: true,
                winLossByGameTypeTitle: "Macau Game Win Loss");

        private static WinLossGameTypeInfoViewModel ExpectedNG2Info =>
             new WinLossGameTypeInfoViewModel(
                isAllowViewGameType: true,
                winLossByGameTypeTitle: "NG2 Win Loss");

        private static WinLossGameTypeInfoViewModel InValidGameTypeInfo =>
            new WinLossGameTypeInfoViewModel(
                isAllowViewGameType: false,
                winLossByGameTypeTitle: string.Empty);

        public static IEnumerable<object[]> WinLossGameTypeInfoViewModelSources => winLossGameTypeInfoViewModel;
    }
}