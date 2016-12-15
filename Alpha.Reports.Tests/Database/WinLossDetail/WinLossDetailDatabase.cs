namespace Alpha.Reports.Tests.Database.WinLossDetail
{
    using System.Data;
    using Helper;

    public class WinLossDetailDatabase
    {
        private static DataSet masterWinLoss = FileHelper.LoadDataSet("\\Database\\WinLossDetail\\MasterWinLossDetailHasData.xml");

        public static DataSet MasterWinLoss
        {
            get
            {
                return masterWinLoss;
            }
        }
    }
}
