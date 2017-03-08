namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.WinLossAnalysis;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    public static class WinLossAnalysisViewModelTestCases
    {
        private static IEnumerable<object[]> winLossAnalysisViewModel = new List<object[]>()
        {
            new object[] { BuildSuperDataTest(), ExpectedSuperGrandTotal },
            new object[] { BuildMasterDataTest(), ExpectedMasterGrandTotal },
            new object[] { BuildAgentDataTest(), ExpectedAgentGrandTotal }
        };

        private static WinLossAnalysisModel ExpectedSuperGrandTotal =>
            new WinLossAnalysisModel(
                rollupKey: null,
                betCount: 20,
                turnover: 20,
                memberNetWinLoss: new NetWinLoss(16, 18),
                agentNetWinLoss: new NetWinLoss(20, 22),
                masterNetWinLoss: new NetWinLoss(24, 26),
                superNetWinLoss: new NetWinLoss(28, 30));

        private static WinLossAnalysisModel ExpectedMasterGrandTotal =>
            new WinLossAnalysisModel(
                rollupKey: null,
                betCount: 20,
                turnover: 20,
                memberNetWinLoss: new NetWinLoss(16, 18),
                agentNetWinLoss: new NetWinLoss(20, 22),
                masterNetWinLoss: new NetWinLoss(24, 26),
                superNetWinLoss: null);

        private static WinLossAnalysisModel ExpectedAgentGrandTotal =>
            new WinLossAnalysisModel(
                rollupKey: null,
                betCount: 20,
                turnover: 20,
                memberNetWinLoss: new NetWinLoss(16, 18),
                agentNetWinLoss: new NetWinLoss(20, 22),
                masterNetWinLoss: null,
                superNetWinLoss: null);

        private static List<WinLossAnalysisModel> BuildSuperDataTest()
        {
            var superWinLossAnalysisData = new List<WinLossAnalysisModel>();
    
            superWinLossAnalysisData.Add(new WinLossAnalysisModel(
                 rollupKey: "02/2017",
                 betCount: 10,
                 turnover: 0,
                 memberNetWinLoss: new NetWinLoss(3, 4),
                 agentNetWinLoss: new NetWinLoss(5, 6),
                 masterNetWinLoss: new NetWinLoss(7, 8),
                 superNetWinLoss: new NetWinLoss(9, 10)));

            superWinLossAnalysisData.Add(new WinLossAnalysisModel(
                 rollupKey: "03/2017",
                 betCount: 10,
                 turnover: 20,
                 memberNetWinLoss: new NetWinLoss(13, 14),
                 agentNetWinLoss: new NetWinLoss(15, 16),
                 masterNetWinLoss: new NetWinLoss(17, 18),
                 superNetWinLoss: new NetWinLoss(19, 20)));

            return superWinLossAnalysisData;
        }

        private static List<WinLossAnalysisModel> BuildMasterDataTest()
        {
            var masterWinLossAnalysisData = new List<WinLossAnalysisModel>();

            masterWinLossAnalysisData.Add(new WinLossAnalysisModel(
                 rollupKey: "02/2017",
                 betCount: 10,
                 turnover: 0,
                 memberNetWinLoss: new NetWinLoss(3, 4),
                 agentNetWinLoss: new NetWinLoss(5, 6),
                 masterNetWinLoss: new NetWinLoss(7, 8),
                 superNetWinLoss: null));

            masterWinLossAnalysisData.Add(new WinLossAnalysisModel(
                 rollupKey: "03/2017",
                 betCount: 10,
                 turnover: 20,
                 memberNetWinLoss: new NetWinLoss(13, 14),
                 agentNetWinLoss: new NetWinLoss(15, 16),
                 masterNetWinLoss: new NetWinLoss(17, 18),
                 superNetWinLoss: null));

            return masterWinLossAnalysisData;
        }

        private static List<WinLossAnalysisModel> BuildAgentDataTest()
        {
            var agentWinLossAnalysisData = new List<WinLossAnalysisModel>();

            agentWinLossAnalysisData.Add(new WinLossAnalysisModel(
                 rollupKey: "02/2017",
                 betCount: 10,
                 turnover: 0,
                 memberNetWinLoss: new NetWinLoss(3, 4),
                 agentNetWinLoss: new NetWinLoss(5, 6),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            agentWinLossAnalysisData.Add(new WinLossAnalysisModel(
                 rollupKey: "03/2017",
                 betCount: 10,
                 turnover: 20,
                 memberNetWinLoss: new NetWinLoss(13, 14),
                 agentNetWinLoss: new NetWinLoss(15, 16),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            return agentWinLossAnalysisData;
        }

        public static IEnumerable<object[]> WinLossAnalysisViewModelSources => winLossAnalysisViewModel;
    }
}