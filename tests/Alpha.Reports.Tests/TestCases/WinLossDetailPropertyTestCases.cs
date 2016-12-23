namespace Alpha.Reports.Tests.TestCases
{
    using System.Collections.Generic;

    public static class WinLossDetailPropertyTestCases
    {
        private static IEnumerable<object[]> allProperties = new List<object[]>()
        {
            new object[] { "CustId" },
            new object[] { "CustName" },
            new object[] { "Turnover" },
            new object[] { "NetTurnover" },
            new object[] { "GrossComm" },
            new object[] { "NumberOfTickets" },
            new object[] { "MemberNetWinLoss.WinLoss" },
            new object[] { "MemberNetWinLoss.Comm" },
            new object[] { "MemberNetWinLoss.Total" },
            new object[] { "AgentNetWinLoss.WinLoss" },
            new object[] { "AgentNetWinLoss.Comm" },
            new object[] { "AgentNetWinLoss.Total" },
            new object[] { "MasterNetWinLoss.WinLoss" },
            new object[] { "MasterNetWinLoss.Comm" },
            new object[] { "MasterNetWinLoss.Total" },
            new object[] { "SuperNetWinLoss.WinLoss" },
            new object[] { "SuperNetWinLoss.Comm" },
            new object[] { "SuperNetWinLoss.Total" }
        };

        public static IEnumerable<object[]> AllProperties
        {
            get
            {
                return allProperties;
            }
        }
    }
}
