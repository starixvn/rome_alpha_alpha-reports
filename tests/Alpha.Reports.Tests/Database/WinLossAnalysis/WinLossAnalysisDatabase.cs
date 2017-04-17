namespace Alpha.Reports.Tests.Database.WinLossAnalysis
{
    using System.Data;
    using Helpers;

    public static class WinLossAnalysisDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossAnalysis\\MasterWinLossAnalysisHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossAnalysis\\AgentWinLossAnalysisHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossAnalysis\\MemberWinLossAnalysisHasData.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet MemberWinLoss => memberWinLoss;
    }
}
