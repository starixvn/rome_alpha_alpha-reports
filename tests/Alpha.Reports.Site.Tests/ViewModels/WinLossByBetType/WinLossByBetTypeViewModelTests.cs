namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinlossByBettype;
    using Site.Requests.WinLossByBettype;
    using Site.ViewModels.WinLossByBettype;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly CompareLogic compareLogic;

        public WinLossByBetTypeViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void Title_WinLossLevel_ReturnCorrectTitle()
        {
            // Arrange
            var expectedTitle = "Win Loss By Bet Type";
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByBettypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today);

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.Title, expectedTitle);
        }

        [Fact]
        public void WinLossViewModel_NullReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrandTotal = new WinLossByBetTypeModel();
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByBettypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today);

            // Act
            var actualGrandTotal = winlossByBettypeViewModel.GrandTotal;

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedReportData = Enumerable.Empty<WinLossByBetTypeModel>();
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByBettypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today);

            // Act
            var actualReportData = winlossByBettypeViewModel.ReportData;

            // Assert
            var result = compareLogic.Compare(actualReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrandTotal = new WinLossByBetTypeModel();
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByBettypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today);

            // Act
            var actualGrandTotal = winlossByBettypeViewModel.GrandTotal;

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }

        private WinLossByBettypeQuery CreateWinlossByBetTypeQuery(byte custLevelId)
        {
            var winLossBettypeQuery = new WinLossByBettypeQuery(userFixture.User);
            winLossBettypeQuery.CustLevelId = custLevelId;
            winLossBettypeQuery.FromDate = DefaultVariables.CurrentDate;
            winLossBettypeQuery.ToDate = DefaultVariables.CurrentDate;

            return winLossBettypeQuery;
        }
    }
}