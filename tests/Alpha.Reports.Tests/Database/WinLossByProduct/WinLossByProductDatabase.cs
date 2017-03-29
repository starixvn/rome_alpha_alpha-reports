namespace Alpha.Reports.Tests.Database.WinLossByProduct
{
    using System.Data;
    using Helpers;

    public static class WinLossByProductDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByProduct\\MasterWinLossByProductHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByProduct\\AgentWinLossByProductHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossByProduct\\MemberWinLossByProductHasData.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet MemberWinLoss => memberWinLoss;
    }
}