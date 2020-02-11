namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Reports.Site.Handlers;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.Reports.Site.ViewModels;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Services;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Utils;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("UserCollection")]
    public class RiskAnalysisIPMatchingHandlerTests
    {
        private readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly RiskAnalysisIPMatchingHandler riskAnalysisIPMatchingHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public RiskAnalysisIPMatchingHandlerTests(UserFixture userFixture)
        {
            riskAnalysisIPMatchingService = Substitute.For<RiskAnalysisIPMatchingService>();

            this.userFixture = userFixture;
            riskAnalysisIPMatchingHandler = new RiskAnalysisIPMatchingHandler(riskAnalysisIPMatchingService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleRiskAnalysisIPMatchingQuery_ValidMessage_ReturnsDefaultData()
        {
            // Arrange
            var message = new RiskAnalysisIPMatchingQuery(userFixture.User);
            message.CustLevelId = (byte)4;
            message.CustId = 1596;
            var reportData = Enumerable.Empty<RiskAnalysisIPMatchingModel>();
            riskAnalysisIPMatchingService.GetRiskAnalysisIPMatchingModel(Arg.Any<RiskAnalysisIPMatchingParam>()).Returns(reportData);

            var expectedResult = new RiskAnalysisIPMatchingViewModel(
                message.SelectedWinlostDate,
                message.MinDate,
                message.MemberCount,
                reportData);

            // Act
            var actualResult = riskAnalysisIPMatchingHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedResult, actualResult);
            Assert.True(result.AreEqual);
        }
    }
}