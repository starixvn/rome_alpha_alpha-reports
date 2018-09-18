namespace Alpha.Reports.Tests.Database.PokerGamesWinLossDetail
{
    using System.Data;
    using Helpers;

    public static class PokerGamesWinLossDetailDatabase
    {
        private static DataSet masterPokerGamesWinLoss = FileHelper.LoadDataSet("\\Database\\PokerGamesWinLossDetail\\MasterPokerGamesWinLossDetailHasData.xml");
        private static DataSet agentPokerGamesWinLoss = FileHelper.LoadDataSet("\\Database\\PokerGamesWinLossDetail\\AgentPokerGamesWinLossDetailHasData.xml");
        private static DataSet memberPokerGamesWinLoss = FileHelper.LoadDataSet("\\Database\\PokerGamesWinLossDetail\\MemberPokerGamesWinLossDetailHasData.xml");

        private static DataSet masterPokerGamesWinLossWinNoTotalTable = FileHelper.LoadDataSet("\\Database\\PokerGamesWinLossDetail\\MasterPokerGamesWinLossDetailHasData WinNoTotalTable.xml");

        public static DataSet MasterPokerGamesWinLoss => masterPokerGamesWinLoss;

        public static DataSet AgentPokerGamesWinLoss => agentPokerGamesWinLoss;

        public static DataSet MemberPokerGamesWinLoss => memberPokerGamesWinLoss;

        public static DataSet MasterPokerGamesWinLossWinNoTotalTable => masterPokerGamesWinLossWinNoTotalTable;
    }
}
