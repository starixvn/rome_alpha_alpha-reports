namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
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
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
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
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FromDate_SupportFromDate_ReturnsSupportedFromDate(bool isHistoricReport)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = testContextFixture.CurrentDate;

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, isHistoricReport, testContextFixture);
            var expectedDate = fromDate;

            // Act
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate(bool isHistoricReport)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var winLossDetailParam = new WinLossDetailParam(
                    0,
                    0,
                    0,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    Enumerable.Empty<int>(),
                    reportLevel,
                    string.Empty,
                    false,
                    isHistoricReport,
                    testContextFixture);

            var expectedDate = testContextFixture.CurrentDate;

            // Act
            var actualDate = winLossDetailParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToDate_SupportToDate_ReturnsSupportedToDate(bool isHistoricReport)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime toDate = testContextFixture.CurrentDate;

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, isHistoricReport, testContextFixture);
            var expectedDate = toDate;

            // Act
            var actualDate = winLossDetailParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ReportLevel_Always_ReturnsReportLevelFromConstructor()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false, testContextFixture);
            var expectedReportLevel = reportLevel;

            // Act
            var actualReportLevel = winLossDetailParam.ReportLevel;

            // Assert
            Assert.Equal(expectedReportLevel, actualReportLevel);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsDescendingSort_Always_ReturnsISDescendingSortFromConstructor(bool isDescendingSort)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                reportLevel,
                string.Empty,
                isDescendingSort,
                false,
                testContextFixture);
            var expectedIsDescendingSort = isDescendingSort;

            // Act
            var actualIsDescendingSort = winLossDetailParam.IsDescendingSort;

            // Assert
            Assert.Equal(expectedIsDescendingSort, actualIsDescendingSort);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1,2")]
        public void UserSelectedProductIds_Always_ReturnsUserSelectedProductIdsFromConstructor(string userSelectedProductIds)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var userSelectedProductIdList = userSelectedProductIds.Split(',').Select(id => Converter.ToInt(id)).ToList();

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, userSelectedProductIdList, reportLevel, string.Empty, false, false, testContextFixture);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = winLossDetailParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }
    }
}
