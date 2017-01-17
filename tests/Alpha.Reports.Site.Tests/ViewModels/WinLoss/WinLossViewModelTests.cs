namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using TestCases;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class WinLossViewModelTests
    { 
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private CompareLogic compareLogic;

        public WinLossViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
            this.userFixture = userFixture;
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
            var message = CreateWinlossQuery(custLevelId);
            var winlossViewModel = new WinLossViewModel(
                 Enumerable.Empty<WinLossModel>(),
                 message,
                 fixture.Create<WinLossParam>(),
                 Enumerable.Empty<CustInfo>(),
                 Enumerable.Empty<SelectListItem>());
            
            // Act & Assert
            Assert.Equal(winlossViewModel.Title, expectedTitle);
        }

        [Theory]
        [MemberData("WinLossViewModelSources", MemberType = typeof(WinLossViewModelTestCases))]
        public void WinLossViewModel_ValidReportData_ReturnsCorrectGrandTotal(
           List<WinLossModel> winlossData,
           WinLossModel expectedGrantTotal)
        {
            // Arrange
            var message = CreateWinlossQuery(4);
            var winlossViewModel = new WinLossViewModel(
                winlossData,
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualGrantTotal = winlossViewModel.GrantTotal;

            // Assert
            var result = compareLogic.Compare(actualGrantTotal, expectedGrantTotal);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_NullReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrantTotal = new WinLossModel();
            var message = CreateWinlossQuery(4);
            var winlossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualGrantTotal = winlossViewModel.GrantTotal;

            // Assert
            var result = compareLogic.Compare(actualGrantTotal, expectedGrantTotal);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedReportData = Enumerable.Empty<WinLossModel>();
            var message = CreateWinlossQuery(4);
            var winlossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());
            
            // Act
            var actualReportData = winlossViewModel.ReportData;

            // Assert
            var result = compareLogic.Compare(actualReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrantTotal = new WinLossModel();
            var message = CreateWinlossQuery(4);
            var winlossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualGrantTotal = winlossViewModel.GrantTotal;

            // Assert
            var result = compareLogic.Compare(actualGrantTotal, expectedGrantTotal);
            Assert.True(result.AreEqual);
        }

        private WinLossQuery CreateWinlossQuery(byte custLevelId)
        {
            var winLossQuery = new WinLossQuery(userFixture.User);
            winLossQuery.CustLevelId = custLevelId;
            winLossQuery.FromDate = DefaultVariables.CurrentDate;
            winLossQuery.ToDate = DefaultVariables.CurrentDate;

            return winLossQuery;
        }
    }
}