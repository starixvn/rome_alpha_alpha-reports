namespace Alpha.Reports.Tests.Database.WinLossByBetType
{
    using System.Data;
    using Helper;

    public static class WinLossByBetTypeDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByBetType\\MasterWinLossByBetTypeHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByBetType\\AgentWinLossByBetTypeHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByBetType\\MemberWinLossByBetTypeHasData.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet MemberWinLoss => memberWinLoss;
    }
}