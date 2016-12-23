namespace Alpha.Reports.Tests.Database.WinLossDetail
{
    using System.Data;
    using Helper;

    public static class WinLossDetailDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData.xml");
        private static DataSet agentWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\AgentWinLossDetailHasData.xml");
        private static DataSet memberWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MemberWinLossDetailHasData.xml");

        private static DataSet masterWinLossWinNoTotalTable = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData WinNoTotalTable.xml");

        public static DataSet MasterWinLoss
        {
            get
            {
                return masterWinLoss;
            }
        }

        public static DataSet AgentWinLoss
        {
            get
            {
                return agentWinLoss;
            }
        }

        public static DataSet MemberWinLoss
        {
            get
            {
                return memberWinLoss;
            }
        }

        public static DataSet MasterWinLossWinNoTotalTable
        {
            get
            {
                return masterWinLossWinNoTotalTable;
            }
        }
    }
}
