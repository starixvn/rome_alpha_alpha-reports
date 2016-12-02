namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Reports.Enumerations;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Params;
    using TestFixtures;
    using Xunit;

    [Collection("ReportCollection")]
    public class WinLossDetailParamTests
    {
        private TestContextFixture testContextFixture;

        public WinLossDetailParamTests(TestContextFixture testContextFixture)
        {
            this.testContextFixture = testContextFixture;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var custId = 1;
            var winLossDetailParam = new WinLossDetailParam(custId, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);

            // Act
            var actual = winLossDetailParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false, testContextFixture);
            var expectedDate = testContextFixture.CurrentDate;

            // Act
            var actual = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actual);
        }

        [Fact]
        public void FromDate_HistoricReportAndNotSupportFromDate_ReturnsFirstDateOfThisMonth()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, true, testContextFixture);
            var expectedDate = testContextFixture.CurrentDate.FirstDayOfMonth();

            // Act
            var actual = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actual);
        }
    }
}
