namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.Winloss;

    public static class WinLossViewModelTestCases
    {
        private static IEnumerable<object[]> winLossViewModel = new List<object[]>()
        {
            new object[] { BuildSuperDataTest(), ExpectedSuperGrandTotal },
            new object[] { BuildMasterDataTest(), ExpectedMasterGrandTotal },
            new object[] { BuildAgentDataTest(), ExpectedAgentGrandTotal }
        };

        private static WinLossModel ExpectedSuperGrandTotal =>
            new WinLossModel(
                custId: 0,
                custName: null,
                turnover: (decimal)375732807.36,
                grossComm: (decimal)2998559.76,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54), firstName: null, lastName: null);

        private static WinLossModel ExpectedMasterGrandTotal =>
            new WinLossModel(
                custId: 0,
                custName: null,
                turnover: (decimal)375732807.36,
                grossComm: (decimal)2998559.76,
                memberNetWinLoss: null,
                agentNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54),
                superNetWinLoss: null, firstName: null, lastName: null);

        private static WinLossModel ExpectedAgentGrandTotal =>
            new WinLossModel(
                custId: 0,
                custName: null,
                turnover: (decimal)6225.00,
                grossComm: (decimal)34.83,
                memberNetWinLoss: new NetWinLoss((decimal)-2576.25, (decimal)21.00),
                agentNetWinLoss: new NetWinLoss((decimal)1267.88, (decimal)-3.38),
                masterNetWinLoss: null,
                superNetWinLoss: null, firstName: null, lastName: null);

        private static List<WinLossModel> BuildSuperDataTest()
        {
            var superWinLossData = new List<WinLossModel>();

            superWinLossData.Add(new WinLossModel(
                 custId: 1382145,
                 custName: "ACGUUS38",
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69), firstName: null, lastName: null));

            superWinLossData.Add(new WinLossModel(
                 custId: 18187823,
                 custName: "ACGUUSOP",
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)9.65),
                 superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15), firstName: null, lastName: null));

            return superWinLossData;
        }

        private static List<WinLossModel> BuildMasterDataTest()
        {
            var masterWinLossData = new List<WinLossModel>();

            masterWinLossData.Add(new WinLossModel(
                 custId: 1382145,
                 custName: "ACGUUS3801",
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69),
                 superNetWinLoss: null, firstName: null, lastName: null));

            masterWinLossData.Add(new WinLossModel(
                 custId: 18187823,
                 custName: "ACGUUS3802",
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)9.65),
                 masterNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15),
                 superNetWinLoss: null, firstName: null, lastName: null));

            return masterWinLossData;
        }

        private static List<WinLossModel> BuildAgentDataTest()
        {
            var agentWinLossData = new List<WinLossModel>();

            agentWinLossData.Add(new WinLossModel(
                 custId: 1382145,
                 custName: "ACGUUS3801",
                 turnover: (decimal)4000.00,
                 grossComm: (decimal)21.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-2025.00, (decimal)21.00),
                 agentNetWinLoss: new NetWinLoss((decimal)992.25, (decimal)-10.29),
                 masterNetWinLoss: null,
                 superNetWinLoss: null, firstName: null, lastName: null));

            agentWinLossData.Add(new WinLossModel(
                 custId: 18187823,
                 custName: "ACGUUS3838006",
                 turnover: (decimal)2225.00,
                 grossComm: (decimal)13.83,
                 memberNetWinLoss: new NetWinLoss((decimal)-551.25, (decimal)0.00),
                 agentNetWinLoss: new NetWinLoss((decimal)275.63, (decimal)6.91),
                 masterNetWinLoss: null,
                 superNetWinLoss: null, firstName: null, lastName: null));

            return agentWinLossData;
        }

        public static IEnumerable<object[]> WinLossViewModelSources => winLossViewModel;
    }
}