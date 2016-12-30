namespace Alpha.Reports.Tests.Database.WinLoss
{
    using System.Data;
    using Helper;

    public static class WinLossDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLoss\\MasterWinLossHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLoss\\AgentWinLossHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLoss\\MemberWinLossHasData.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet MemberWinLoss => memberWinLoss;
    }
}