namespace Alpha.Reports.Tests.Database.WinLossByGameType
{
    using System.Data;
    using Helper;

    public static class WinLossByGameTypeDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByGameType\\MasterWinLossByGameTypeHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByGameType\\AgentWinLossByGameTypeHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByGameType\\MemberWinLossByGameTypeHasData.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet MemberWinLoss => memberWinLoss;
    }
}