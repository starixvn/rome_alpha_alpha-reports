namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;

    public static class WinLossByProductViewModelTestCases
    {
        private static IEnumerable<object[]> winLossByProductViewModel = new List<object[]>()
        {
            new object[] { BuildSuperDataTest(), ExpectedSuperGrantTotal },
            new object[] { BuildMasterDataTest(), ExpectedMasterGrantTotal },
            new object[] { BuildAgentDataTest(), ExpectedAgentGrantTotal }
        };

        private static WinLossByBetTypeModel ExpectedSuperGrantTotal =>
            new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: null,
                productId: 0,
                liveIndicator: false,
                betCount: 6 * 2,
                turnover: (decimal)375732807.36 * 2,
                grossComm: (decimal)2998559.76 * 2,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss((decimal)-111173383.51 * 2),
                superNetWinLoss: new NetWinLoss((decimal)74561618.35 * 2, (decimal)-374728.54 * 2));

        private static WinLossByBetTypeModel ExpectedMasterGrantTotal =>
            new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: null,
                productId: 0,
                liveIndicator: false,
                betCount: 6 * 2,
                turnover: (decimal)375732807.36 * 2,
                grossComm: (decimal)2998559.76 * 2,
                memberNetWinLoss: null,
                agentNetWinLoss: new NetWinLoss((decimal)-111173383.51 * 2),
                masterNetWinLoss: new NetWinLoss((decimal)74561618.35 * 2, (decimal)-374728.54 * 2),
                superNetWinLoss: null);

        private static WinLossByBetTypeModel ExpectedAgentGrantTotal =>
            new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: null,
                productId: 0,
                liveIndicator: false,
                betCount: 6 * 2,
                turnover: (decimal)6225.00 * 2,
                grossComm: (decimal)34.83 * 2,
                memberNetWinLoss: new NetWinLoss((decimal)-2576.25 * 2, (decimal)21.00 * 2),
                agentNetWinLoss: new NetWinLoss((decimal)1267.88 * 2, (decimal)-3.38 * 2),
                masterNetWinLoss: null,
                superNetWinLoss: null);

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private static List<WinLossByProductModel> BuildSuperDataTest()
        {
            var winLossByProductData = new List<WinLossByProductModel>();

            var winLossByBetTypeLiveCasinoData = new List<WinLossByBetTypeModel>()
                {
                     new WinLossByBetTypeModel(
                     betTypeId: 1,
                     betTypeName: "Baccarat",
                     productId: 0,
                     liveIndicator: false,
                     betCount: 4,
                     turnover: (decimal)375732407.36,
                     grossComm: (decimal)2998557.31,
                     memberNetWinLoss: null,
                     agentNetWinLoss: null,
                     masterNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                     superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69)),

                     new WinLossByBetTypeModel(
                     betTypeId: 2,
                     betTypeName: "Blackjack",
                     productId: 0,
                     liveIndicator: false,
                     betCount: 2,
                     turnover: (decimal)400.00,
                     grossComm: (decimal)2.45,
                     memberNetWinLoss: null,
                     agentNetWinLoss: null,
                     masterNetWinLoss: new NetWinLoss((decimal)9.65),
                     superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15))
                };

            var winLossByBetTypeVirtualSportData = new List<WinLossByBetTypeModel>()
                {
                     new WinLossByBetTypeModel(
                     betTypeId: 1231,
                     betTypeName: "Win",
                     productId: 0,
                     liveIndicator: false,
                     betCount: 4,
                     turnover: (decimal)375732407.36,
                     grossComm: (decimal)2998557.31,
                     memberNetWinLoss: null,
                     agentNetWinLoss: null,
                     masterNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                     superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69)),

                     new WinLossByBetTypeModel(
                     betTypeId: 1233,
                     betTypeName: "Win Place",
                     productId: 0,
                     liveIndicator: false,
                     betCount: 2,
                     turnover: (decimal)400.00,
                     grossComm: (decimal)2.45,
                     memberNetWinLoss: null,
                     agentNetWinLoss: null,
                     masterNetWinLoss: new NetWinLoss((decimal)9.65),
                     superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15))
                };

            var winLossByLiveCasino = new WinLossByProductModel(
                productId: 71,
                productName: "Live Casino",
                winLossByBetTypeData: winLossByBetTypeLiveCasinoData);

            winLossByProductData.Add(winLossByLiveCasino);

            return new List<WinLossByProductModel>()
                        {
                            new WinLossByProductModel(
                            productId: 71,
                            productName: "Live Casino",
                            winLossByBetTypeData: winLossByBetTypeLiveCasinoData),

                            new WinLossByProductModel(
                            productId: 5,
                            productName: "Virtual Sport",
                            winLossByBetTypeData: winLossByBetTypeVirtualSportData)
                        };
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private static List<WinLossByProductModel> BuildMasterDataTest()
        {
            var winLossByBetTypeLiveCasinoData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(
                 betTypeId: 1,
                 betTypeName: "Baccarat",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 4,
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69),
                 superNetWinLoss: null),

                new WinLossByBetTypeModel(
                 betTypeId: 2,
                 betTypeName: "Blackjack",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 2,
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)9.65),
                 masterNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15),
                 superNetWinLoss: null)
            };

            var winLossByBetTypeVirtualSportData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(
                 betTypeId: 1231,
                 betTypeName: "Win",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 4,
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69),
                 superNetWinLoss: null),

                new WinLossByBetTypeModel(
                 betTypeId: 1233,
                 betTypeName: "Win Place",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 2,
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)9.65),
                 masterNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15),
                 superNetWinLoss: null)
            };

            return new List<WinLossByProductModel>()
                        {
                            new WinLossByProductModel(
                            productId: 71,
                            productName: "Live Casino",
                            winLossByBetTypeData: winLossByBetTypeLiveCasinoData),

                            new WinLossByProductModel(
                            productId: 5,
                            productName: "Virtual Sport",
                            winLossByBetTypeData: winLossByBetTypeVirtualSportData)
                        };
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private static List<WinLossByProductModel> BuildAgentDataTest()
        {
            var winLossByBetTypeVirtualSportData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(
                 betTypeId: 1231,
                 betTypeName: "Win",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 4,
                 turnover: (decimal)4000.00,
                 grossComm: (decimal)21.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-2025.00, (decimal)21.00),
                 agentNetWinLoss: new NetWinLoss((decimal)992.25, (decimal)-10.29),
                 masterNetWinLoss: null,
                 superNetWinLoss: null),

                new WinLossByBetTypeModel(
                 betTypeId: 1233,
                 betTypeName: "Win Place",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 2,
                 turnover: (decimal)2225.00,
                 grossComm: (decimal)13.83,
                 memberNetWinLoss: new NetWinLoss((decimal)-551.25, (decimal)0.00),
                 agentNetWinLoss: new NetWinLoss((decimal)275.63, (decimal)6.91),
                 masterNetWinLoss: null,
                 superNetWinLoss: null)
            };

            var winLossByBetTypeLiveCasinoData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(
                 betTypeId: 1,
                 betTypeName: "Baccarat",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 4,
                 turnover: (decimal)4000.00,
                 grossComm: (decimal)21.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-2025.00, (decimal)21.00),
                 agentNetWinLoss: new NetWinLoss((decimal)992.25, (decimal)-10.29),
                 masterNetWinLoss: null,
                 superNetWinLoss: null),

                new WinLossByBetTypeModel(
                 betTypeId: 2,
                 betTypeName: "Blackjack",
                 productId: 0,
                 liveIndicator: false,
                 betCount: 2,
                 turnover: (decimal)2225.00,
                 grossComm: (decimal)13.83,
                 memberNetWinLoss: new NetWinLoss((decimal)-551.25, (decimal)0.00),
                 agentNetWinLoss: new NetWinLoss((decimal)275.63, (decimal)6.91),
                 masterNetWinLoss: null,
                 superNetWinLoss: null)
            };

            return new List<WinLossByProductModel>()
                        {
                            new WinLossByProductModel(
                            productId: 71,
                            productName: "Live Casino",
                            winLossByBetTypeData: winLossByBetTypeLiveCasinoData),

                            new WinLossByProductModel(
                            productId: 5,
                            productName: "Virtual Sport",
                            winLossByBetTypeData: winLossByBetTypeVirtualSportData)
                        };
        }

        public static IEnumerable<object[]> WinLossByProductViewModelSources => winLossByProductViewModel;
    }
}