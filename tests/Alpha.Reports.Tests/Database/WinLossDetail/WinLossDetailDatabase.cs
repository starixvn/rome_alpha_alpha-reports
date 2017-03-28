namespace Alpha.Reports.Tests.Database.WinLossDetail
{
    using System.Data;
    using Helpers;

    public static class WinLossDetailDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\AgentWinLossDetailHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MemberWinLossDetailHasData.xml");

        private static DataSet masterWinLossWinNoTotalTable = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData WinNoTotalTable.xml");

        public static DataSet MasterWinLoss => masterWinLoss;

        public static DataSet AgentWinLoss => agentWinLoss;

        public static DataSet MemberWinLoss => memberWinLoss;

        public static DataSet MasterWinLossWinNoTotalTable => masterWinLossWinNoTotalTable;
    }
}
