namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Enumerations;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredSportResultHandlerTests
    {
        private readonly WinLossService reportService;
        private readonly EnteredSportResultHandler enteredSportResultHandler;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public EnteredSportResultHandlerTests()
        {
            reportService = Substitute.For<WinLossService>();
            user = Substitute.For<IUser>();
            enteredSportResultHandler = new EnteredSportResultHandler(reportService);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
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
            var message = new EnteredSportResultQuery(user);
            message.CustLevelId = (byte)4;
            message.CustId = 1596;
            var reportData = Enumerable.Empty<EnteredSportResult>();
            reportService.GetEnteredSportResults().Returns(reportData);

            var expectedResult = new EnteredResultViewModel
            {
                CustLevel = Enumeration.FromValue<UserLevel>(4),
                FromDate = DateTimeManager.Now,
                ToDate = DateTimeManager.Now,
                Language = message.Language,
                ReportData = reportData,
                UserSelectedDateRangeOption = 0
            };

            // Act
            var actualResult = enteredSportResultHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedResult, actualResult);
            Assert.True(result.AreEqual);
        }
    }
}