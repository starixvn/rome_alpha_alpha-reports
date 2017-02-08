namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinlossByGameType;

    public static class WinLossByGameTypeViewModelTestCases
    {
        private static IEnumerable<object[]> winLossByGameTypeViewModel = new List<object[]>()
        {
            new object[] { BuildSuperDataTest(), ExpectedSuperGrantTotal },
            new object[] { BuildMasterDataTest(), ExpectedMasterGrantTotal },
            new object[] { BuildAgentDataTest(), ExpectedAgentGrantTotal }
        };

        private static WinLossByGameTypeModel ExpectedSuperGrantTotal =>
            new WinLossByGameTypeModel(
                gameTypeId: 0,
                gameTypeName: string.Empty,
                turnover: (decimal)375732807.36,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54));

        private static WinLossByGameTypeModel ExpectedMasterGrantTotal =>
            new WinLossByGameTypeModel(
                gameTypeId: 0,
                gameTypeName: string.Empty,
                turnover: (decimal)375732807.36,
                memberNetWinLoss: null,
                agentNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54),
                superNetWinLoss: null);

        private static WinLossByGameTypeModel ExpectedAgentGrantTotal =>
            new WinLossByGameTypeModel(
                gameTypeId: 0,
                gameTypeName: string.Empty,
                turnover: (decimal)6225.00,
                memberNetWinLoss: new NetWinLoss((decimal)-2576.25, (decimal)21.00),
                agentNetWinLoss: new NetWinLoss((decimal)1267.88, (decimal)-3.38),
                masterNetWinLoss: null,
                superNetWinLoss: null);

        private static List<WinLossByGameTypeModel> BuildSuperDataTest()
        {
            var superWinLossData = new List<WinLossByGameTypeModel>();

            superWinLossData.Add(new WinLossByGameTypeModel(
                 gameTypeId: 2176,
                 gameTypeName: "Sea Orchestra",
                 turnover: (decimal)375732407.36,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69)));

            superWinLossData.Add(new WinLossByGameTypeModel(
                 gameTypeId: 2301,
                 gameTypeName: "(Mobile)Baccarat ",
                 turnover: (decimal)400.00,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)9.65),
                 superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15)));

            return superWinLossData;
        }

        private static List<WinLossByGameTypeModel> BuildMasterDataTest()
        {
            var masterWinLossData = new List<WinLossByGameTypeModel>();

            masterWinLossData.Add(new WinLossByGameTypeModel(
                 gameTypeId: 2176,
                 gameTypeName: "Sea Orchestra",
                 turnover: (decimal)375732407.36,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69),
                 superNetWinLoss: null));

            masterWinLossData.Add(new WinLossByGameTypeModel(
                 gameTypeId: 2301,
                 gameTypeName: "(Mobile)Baccarat ",
                 turnover: (decimal)400.00,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)9.65),
                 masterNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15),
                 superNetWinLoss: null));

            return masterWinLossData;
        }

        private static List<WinLossByGameTypeModel> BuildAgentDataTest()
        {
            var agentWinLossData = new List<WinLossByGameTypeModel>();

            agentWinLossData.Add(new WinLossByGameTypeModel(
                 gameTypeId: 2176,
                 gameTypeName: "Sea Orchestra",
                 turnover: (decimal)4000.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-2025.00, (decimal)21.00),
                 agentNetWinLoss: new NetWinLoss((decimal)992.25, (decimal)-10.29),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            agentWinLossData.Add(new WinLossByGameTypeModel(
                 gameTypeId: 2301,
                 gameTypeName: "(Mobile)Baccarat ",
                 turnover: (decimal)2225.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-551.25, (decimal)0.00),
                 agentNetWinLoss: new NetWinLoss((decimal)275.63, (decimal)6.91),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            return agentWinLossData;
        }

        public static IEnumerable<object[]> WinLossByGameTypeViewModelSources => winLossByGameTypeViewModel;
    }
}