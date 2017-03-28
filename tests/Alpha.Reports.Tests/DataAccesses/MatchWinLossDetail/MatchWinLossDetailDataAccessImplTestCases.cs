namespace Alpha.Reports.Tests.DataAccesses.MatchWinLossDetail
{
    using System.Collections.Generic;
    using Database.MatchWinLossDetail;
    using Reports.Models;

    public static class MatchWinLossDetailDataAccessImplTestCases
    {
        private static IEnumerable<object[]> reportByLevel = new List<object[]>
        {
            new object[] { UserLevel.Super,  MatchWinLossDetailDatabase.SuperWinLoss },
            new object[] { UserLevel.Master,  MatchWinLossDetailDatabase.MasterWinLoss },
            new object[] { UserLevel.Agent,  MatchWinLossDetailDatabase.AgentWinLoss },
        };

        public static IEnumerable<object[]> ReportByLevel => reportByLevel;
    }
}
