namespace Alpha.Reports.Site.Tests.Handlers.MatchWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using RemotingRef;
    using Reports.Models.MatchWinLoss;
    using Services;
    using Site.Handlers.MatchWinLoss;
    using Site.Requests.MatchWinLoss;
    using Site.ViewModels.MatchWinLoss;
    using Utils;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("UserCollection")]
    public class MatchWinLossHandlerTests
    {
        private readonly MatchWinLossService reportService;
        private readonly MatchWinLossHandler matchWinLossHandler;
        private readonly UserFixture userFixture;
        private readonly Fixture anyValueGenerator;

        public MatchWinLossHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<MatchWinLossService>();
            var cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetTeamName(Arg.Any<int>()).Returns(callInfo => "Team " + callInfo.Args()[0]);
            cacheRefService.GetBetType(Arg.Any<int>()).Returns(callInfo => "Bet Type " + callInfo.Args()[0]);

            matchWinLossHandler = new MatchWinLossHandler(reportService, cacheRefService);

            this.userFixture = userFixture;
            anyValueGenerator = new Fixture();

            SystemTime.Today = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariables.BalanceUpDate);
        }

        [Fact]
        public void HandleMatchWinLossQuery_Always_CallsServiceToGetDataWithCustIdFromQuery()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            // Act
            matchWinLossHandler.Handle(message);

            // Assert
            reportService.Received().GetMatchWinLoss(Arg.Is<MatchWinLossParam>(param
                => param != null
                && param.CustId == message.CustId));
        }

        [Fact]
        public void HandleMatchWinLossQuery_DefaultFromDate_CallsServiceToGetDataWithFromDateIsCurrentDate()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            // Act
            matchWinLossHandler.Handle(message);

            // Assert
            reportService.Received().GetMatchWinLoss(Arg.Is<MatchWinLossParam>(param
                => param != null
                && param.FromDate == DefaultVariables.CurrentDate));
        }

        [Fact]
        public void HandleMatchWinLossQuery_DefaultToDate_CallsServiceToGetDataWithToDateIsCurrentDate()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            // Act
            matchWinLossHandler.Handle(message);

            // Assert
            reportService.Received().GetMatchWinLoss(Arg.Is<MatchWinLossParam>(param
                => param != null
                && param.ToDate == DefaultVariables.CurrentDate));
        }

        [Fact]
        public void HandleMatchWinLossQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            var rawData = new List<MatchWinLossModel>
            {
                new MatchWinLossModel(1, 2, 3, 81, false, false, "Match Code", "Show Time", 0, 0, 0),
                new MatchWinLossModel(1, 2, 3, 85, false, false, "Match Code", "Show Time", 0, 0, 0),
                new MatchWinLossModel(1, 2, 3, 86, false, false, "Match Code", "Show Time", 0, 0, 0),
                new MatchWinLossModel(1, 2, 3, 86, true, false, "Match Code", "Show Time", 0, 0, 0),
                new MatchWinLossModel(1, 2, 3, 1105, false, false, "Match Code", "Show Time", 0, 0, 0),
                new MatchWinLossModel(1, 2, 3, 1105, false, true, "Match Code", "Show Time", 0, 0, 0)
            };

            reportService.GetMatchWinLoss(Arg.Any<MatchWinLossParam>()).Returns(rawData);

            var expectedViewModel = new MatchWinLossReportViewModel(
                message,
                anyValueGenerator.Create<DateTime>(),
                anyValueGenerator.Create<MatchWinLossParam>(),
                new List<MatchWinLossViewModel>
                {
                    new MatchWinLossViewModel("Number Game No Match Code", "Bet Type 81", rawData[0]),
                    new MatchWinLossViewModel("Number Game No Match Code", "Next Bet Type 85", rawData[1]),
                    new MatchWinLossViewModel("Number Game No Match Code", "FT. Bet Type 86", rawData[2]),
                    new MatchWinLossViewModel("Number Game No Match Code", "Next Bet Type 86", rawData[3]),
                    new MatchWinLossViewModel("Team 2 -vs- Team 3", "Bet Type 1105", rawData[4]),
                    new MatchWinLossViewModel("Team 2 (N) -vs- Team 3", "Bet Type 1105", rawData[5])
                },
                anyValueGenerator.Create<MatchWinLossViewModel>());

            // Act
            var actualViewModel = matchWinLossHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel.ReportData, actualViewModel.ReportData);
        }

        [Fact]
        public void HandleMatchWinLossQuery_NullReportData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            List<MatchWinLossModel> rawData = null;

            reportService.GetMatchWinLoss(Arg.Any<MatchWinLossParam>()).Returns(rawData);

            var expectedViewModel = new MatchWinLossReportViewModel(
                message,
                anyValueGenerator.Create<DateTime>(),
                anyValueGenerator.Create<MatchWinLossParam>(),
                Enumerable.Empty<MatchWinLossViewModel>(),
                anyValueGenerator.Create<MatchWinLossViewModel>());

            // Act
            var actualViewModel = matchWinLossHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel.ReportData, actualViewModel.ReportData);
        }

        [Fact]
        public void HandlerMatchWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossHandler.Handle(nullMessage));
        }
    }
}
