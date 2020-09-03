namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Alpha.Web.Helpers;
    using Constants;
    using Enumerations;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class EnteredSportResultHandlerTests
    {
        private readonly WinLossService reportService;
        private readonly BrowserHelper browserHelper;
        private readonly EnteredSportResultHandler enteredSportResultHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public EnteredSportResultHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            reportService = Substitute.For<WinLossService>();
            browserHelper = Substitute.For<BrowserHelper>();
            enteredSportResultHandler = new EnteredSportResultHandler(reportService, browserHelper);
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleEnteredResultQuery_NullMessage_ThrowsException()
        {
            // Arrange
            EnteredSportResultQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => enteredSportResultHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleEnteredResultQuery_ValidMessage_ReturnsDefaultData()
        {
            // Arrange
            var message = new EnteredSportResultQuery(userFixture.User)
            {
                CustLevelId = 4,
                CustId = 1596
            };

            var reportData = Enumerable.Empty<EnteredSportResult>();
            reportService.GetEnteredSportResults().Returns(reportData);

            var expectedResult = new EnteredResultViewModel(message, reportData);

            // Act
            var actualResult = enteredSportResultHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedResult, actualResult);
            Assert.True(result.AreEqual);
        }
    }
}