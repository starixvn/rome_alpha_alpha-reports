namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models.Winloss;
    using Reports.Models.WinlossByBettype;
    using Reports.Models.WinLossDetail;

    public static class WinLossByBettypeViewModelTestCases
    {
        private static IEnumerable<object[]> winLossByBettypeViewModel = new List<object[]>()
        {
            new object[] { BuildSuperDataTest(), ExpectedSuperGrantTotal },
            new object[] { BuildMasterDataTest(), ExpectedMasterGrantTotal },
            new object[] { BuildAgentDataTest(), ExpectedAgentGrantTotal }
        };

        private static WinlossByBettypeModel ExpectedSuperGrantTotal =>
            new WinlossByBettypeModel(
                custId: 0,
                custName: "Total",
                numberOfTickets: 6,
                turnover: (decimal)375732807.36,
                grossComm: (decimal)2998559.76,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54));

        private static WinlossByBettypeModel ExpectedMasterGrantTotal =>
            new WinlossByBettypeModel(
                custId: 0,
                custName: "Total",
                numberOfTickets: 6,
                turnover: (decimal)375732807.36,
                grossComm: (decimal)2998559.76,
                memberNetWinLoss: null,
                agentNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54),
                superNetWinLoss: null);

        private static WinlossByBettypeModel ExpectedAgentGrantTotal =>
            new WinlossByBettypeModel(
                custId: 0,
                custName: "Total",
                numberOfTickets: 6,
                turnover: (decimal)6225.00,
                grossComm: (decimal)34.83,
                memberNetWinLoss: new NetWinLoss((decimal)-2576.25, (decimal)21.00),
                agentNetWinLoss: new NetWinLoss((decimal)1267.88, (decimal)-3.38),
                masterNetWinLoss: null,
                superNetWinLoss: null);

        private static List<WinlossByBettypeModel> BuildSuperDataTest()
        {
            var superWinLossData = new List<WinlossByBettypeModel>();

            superWinLossData.Add(new WinlossByBettypeModel(
                 custId: 1382145,
                 custName: "ACGUUS38",
                 numberOfTickets: 4,
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69)));

            superWinLossData.Add(new WinlossByBettypeModel(
                 custId: 18187823,
                 custName: "ACGUUSOP",
                 numberOfTickets: 2,
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)9.65),
                 superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15)));

            return superWinLossData;
        }

        private static List<WinlossByBettypeModel> BuildMasterDataTest()
        {
            var masterWinLossData = new List<WinlossByBettypeModel>();

            masterWinLossData.Add(new WinlossByBettypeModel(
                 custId: 1382145,
                 custName: "ACGUUS3801",
                 numberOfTickets: 4,
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69),
                 superNetWinLoss: null));

            masterWinLossData.Add(new WinlossByBettypeModel(
                 custId: 18187823,
                 custName: "ACGUUS3802",
                 numberOfTickets: 2,
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)9.65),
                 masterNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15),
                 superNetWinLoss: null));

            return masterWinLossData;
        }

        private static List<WinlossByBettypeModel> BuildAgentDataTest()
        {
            var agentWinLossData = new List<WinlossByBettypeModel>();

            agentWinLossData.Add(new WinlossByBettypeModel(
                 custId: 1382145,
                 custName: "ACGUUS3838000",
                 numberOfTickets: 4,
                 turnover: (decimal)4000.00,
                 grossComm: (decimal)21.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-2025.00, (decimal)21.00),
                 agentNetWinLoss: new NetWinLoss((decimal)992.25, (decimal)-10.29),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            agentWinLossData.Add(new WinlossByBettypeModel(
                 custId: 18187823,
                 custName: "ACGUUS3838006",
                 numberOfTickets: 2,
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