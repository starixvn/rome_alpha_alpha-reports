namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Reports.Site.Handlers;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.Reports.Site.ViewModels;
    using AlphaPlus.Utilities.IP;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Services;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Utils;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("UserCollection")]
    public class RiskAnalysisIPMatchingBaseHandlerTests
    {
        private readonly IPService ipService;
        private readonly ReportDateService reportDateService;
        private readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly RiskAnalysisIPMatchingBaseHandler riskAnalysisIPMatchingBaseHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public RiskAnalysisIPMatchingBaseHandlerTests(UserFixture userFixture)
        {
            ipService = Substitute.For<IPService>();

            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetsDay);

            riskAnalysisIPMatchingService = Substitute.For<RiskAnalysisIPMatchingService>();

            this.userFixture = userFixture;
            riskAnalysisIPMatchingBaseHandler = new RiskAnalysisIPMatchingBaseHandler(riskAnalysisIPMatchingService, ipService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void BuildRiskAnalysisIPMatchingViewModel_ValidMessage_ReturnsDefaultData()
        {
            // Arrange
            ipService.IsVNIP(Arg.Any<string>()).Returns(false);
            var message = new RiskAnalysisIPMatchingQuery(userFixture.User)
            {
                CustLevelId = 4,
                CustId = 1596
            };

            var reportData = new List<RiskAnalysisIPMatchingModel>
            {
                new RiskAnalysisIPMatchingModel("192.168.1.1", 5, 6, 7, 8, 9, 10),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 5, 6, 7, 8, 9, 10)
            };

            riskAnalysisIPMatchingService.GetRiskAnalysisIPMatchingModel(Arg.Any<RiskAnalysisIPMatchingParam>()).Returns(reportData);

            var expectedResult = new RiskAnalysisIPMatchingViewModel(
                message.SelectedWinlostDate,
                message.MinDate,
                message.MemberCount,
                reportData,
                new RiskAnalysisIPMatchingModel("Total", 10, 12, 14, 16, 18, 20));

            // Act
            var actualResult = riskAnalysisIPMatchingBaseHandler.BuildRiskAnalysisIPMatchingViewModel(message);

            // Assert
            var result = compareLogic.Compare(expectedResult, actualResult);
            Assert.True(result.AreEqual);
        }
    }
}