namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.WinlossByBettype;

    public static class WinLossByBettypeViewModelTestCases
    {
        private static IEnumerable<object[]> winLossByBettypeViewModel = new List<object[]>()
        {
            new object[] { BuildSuperDataTest(), ExpectedSuperGrantTotal },
            new object[] { BuildMasterDataTest(), ExpectedMasterGrantTotal },
            new object[] { BuildAgentDataTest(), ExpectedAgentGrantTotal }
        };

        private static WinLossByBetTypeModel ExpectedSuperGrantTotal =>
            new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: string.Empty,
                productId: 0,
                liveIndicator: false,
                betCount: 6,
                turnover: (decimal)375732807.36,
                grossComm: (decimal)2998559.76,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54));

        private static WinLossByBetTypeModel ExpectedMasterGrantTotal =>
            new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: string.Empty,
                productId: 0,
                liveIndicator: false,
                betCount: 6,
                turnover: (decimal)375732807.36,
                grossComm: (decimal)2998559.76,
                memberNetWinLoss: null,
                agentNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54),
                superNetWinLoss: null);

        private static WinLossByBetTypeModel ExpectedAgentGrantTotal =>
            new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: string.Empty,
                productId: 0,
                liveIndicator: false,
                betCount: 6,
                turnover: (decimal)6225.00,
                grossComm: (decimal)34.83,
                memberNetWinLoss: new NetWinLoss((decimal)-2576.25, (decimal)21.00),
                agentNetWinLoss: new NetWinLoss((decimal)1267.88, (decimal)-3.38),
                masterNetWinLoss: null,
                superNetWinLoss: null);

        private static List<WinLossByBetTypeModel> BuildSuperDataTest()
        {
            var superWinLossData = new List<WinLossByBetTypeModel>();

            superWinLossData.Add(new WinLossByBetTypeModel(
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
                 superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69)));

            superWinLossData.Add(new WinLossByBetTypeModel(
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
                 superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15)));

            return superWinLossData;
        }

        private static List<WinLossByBetTypeModel> BuildMasterDataTest()
        {
            var masterWinLossData = new List<WinLossByBetTypeModel>();

            masterWinLossData.Add(new WinLossByBetTypeModel(
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
                 superNetWinLoss: null));

            masterWinLossData.Add(new WinLossByBetTypeModel(
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
                 superNetWinLoss: null));

            return masterWinLossData;
        }

        private static List<WinLossByBetTypeModel> BuildAgentDataTest()
        {
            var agentWinLossData = new List<WinLossByBetTypeModel>();

            agentWinLossData.Add(new WinLossByBetTypeModel(
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
                 superNetWinLoss: null));

            agentWinLossData.Add(new WinLossByBetTypeModel(
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
                 superNetWinLoss: null));

            return agentWinLossData;
        }

        public static IEnumerable<object[]> WinLossByBettypeViewModelSources => winLossByBettypeViewModel;
    }
}