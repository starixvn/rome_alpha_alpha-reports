namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;

    public static class ExportWinLossDetailParamTestCases
    {
        private static IEnumerable<object[]> userSelectedProductIdsCases = new List<object[]>()
        {
            new object[] { "1", new List<int> { 1 } },
            new object[] { "1,2", new List<int> { 1, 2 } },
            new object[] { "1,2,a", new List<int> { 1, 2, 0 } },
            new object[] { "1,2,3a", new List<int> { 1, 2, 0 } }
        };

        public static IEnumerable<object[]> UserSelectedProductIdsCases
        {
            get
            {
                return userSelectedProductIdsCases;
            }
        }
    }
}
