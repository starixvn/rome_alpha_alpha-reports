namespace Alpha.Reports.Tests.Database.MatchWinLossDetail
{
    using System.Data;
    using Helpers;

    public static class MatchWinLossDetailDatabase
    {
        private static DataSet superWinLoss = FileHelper.LoadDataSet("\\Database\\MatchWinLossDetail\\SuperMatchWinLossDetailHasData.xml");
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\MatchWinLossDetail\\MasterMatchWinLossDetailHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\MatchWinLossDetail\\AgentMatchWinLossDetailHasData.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet SuperWinLoss => superWinLoss;
    }
}
