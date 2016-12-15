namespace Alpha.Reports.Tests.Database.WinLossDetail
{
    using System.Data;
    using Helper;

    public class WinLossDetailDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData.xml");

        private static DataSet masterWinLossWinNoTotalTable = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData WinNoTotalTable.xml");

        public static DataSet MasterWinLoss
        {
            get
            {
                return masterWinLoss;
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
