namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Enumerations;
    using Extensions;
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Site.ViewModels.WinLoss;
    using TestCases;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossViewModelTests
    {
        private CompareLogic compareLogic;

        public WinLossViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
        }

        [Theory]
        [InlineData(4, "Master Win Loss")]
        [InlineData(3, "Agent Win Loss")]
        [InlineData(2, "Member Win Loss")]
        public void Title_WinLossLevel_ReturnCorrectTitle(
            byte custLevelId,
            string expectedTitle)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossViewModel = new WinLossViewModel(new List<WinLossModel>(), custLevel, Enumerable.Empty<CustInfo>());

            // Act & Assert
            Assert.Equal(winLossViewModel.Title, expectedTitle);
        }

        [Theory]
        [MemberData("WinLossViewModelSources", MemberType = typeof(WinLossViewModelTestCases))]
        public void WinLossViewModel_ValidReportData_ReturnsCorrectGrandTotal(
           List<WinLossModel> winlossData,
           WinLossModel expectedGrantTotal)
        {
            // Arrange

            // Act
            var actualGrantTotal = new WinLossViewModel(winlossData, UserLevel.Super, Enumerable.Empty<CustInfo>()).GrantTotal;

            // Assert
            AssertExtension.Equal(actualGrantTotal, expectedGrantTotal);
        }

        [Fact]
        public void WinLossViewModel_NullReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrantTotal = new WinLossModel();

            // Act
            var actualGrantTotal = new WinLossViewModel(null, UserLevel.Super, Enumerable.Empty<CustInfo>()).GrantTotal;

            // Assert
            var result = compareLogic.Compare(actualGrantTotal, expectedGrantTotal);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedReportData = Enumerable.Empty<WinLossModel>();

            // Act
            var actualReportData = new WinLossViewModel(new List<WinLossModel>(), UserLevel.Super, Enumerable.Empty<CustInfo>()).ReportData;

            // Assert
            var result = compareLogic.Compare(actualReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrantTotal = new WinLossModel();

            // Act
            var actualGrantTotal = new WinLossViewModel(new List<WinLossModel>(), UserLevel.Super, Enumerable.Empty<CustInfo>()).GrantTotal;

            // Assert
            var result = compareLogic.Compare(actualGrantTotal, expectedGrantTotal);
            Assert.True(result.AreEqual);
        }
    }
}