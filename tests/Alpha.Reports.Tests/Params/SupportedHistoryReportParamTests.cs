namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Reports.Models;
    using Reports.Params;
    using Xunit;

    public class SupportedHistoryReportParamTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Create_Always_SetsCorrectInitialValues(bool isHistoryReport)
        {
            // Act
            var model = new WinLossParam(
                1,
                1,
                "",
                1,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Agent,
                isHistoryReport,
                string.Empty);

            // Assert
            Assert.Equal(isHistoryReport, model.IsHistoryReport);
        }
    }
}
