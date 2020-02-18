namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Extensions;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("UserCollection")]
    public class BetChoiceMatchingJsonHandlerTests
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly CacheRefService cacheRefService;
        private readonly BetChoiceMatchingJsonHandler handler;
        private readonly IUser user;
        private readonly ReportDateService reportDateService;

        public BetChoiceMatchingJsonHandlerTests(UserFixture userFixture)
        {
            user = userFixture.User;
            reportDateService = Substitute.For<ReportDateService>();
            betChoiceMatchingService = Substitute.For<BetChoiceMatchingService>();
            cacheRefService = Substitute.For<CacheRefService>();
            handler = new BetChoiceMatchingJsonHandler(betChoiceMatchingService, cacheRefService);
            var reportDate = new BalanceUpDayAndMoveBetsDay(DateTime.Parse("2019-09-09"), DateTime.Parse("2019-08-08"));
            reportDateService.GetBalanceUpDayAndMoveBetsDay().Returns(reportDate);
        }

        [Fact]
        public void Handle_ReturnsCorrectJsonModel()
        {
            // Arrange
            var message = new BetChoiceMatchingJsonQuery(user, reportDateService)
            {
                WinlostDate = DateTime.Parse("2019-09-10"),
                BetCount = 2
            };
            cacheRefService.GetTeamName(1).Returns("Team 1");
            cacheRefService.GetTeamName(2).Returns("Team 2");
            cacheRefService.GetLeagueName(8).Returns("League 8");
            cacheRefService.GetSportName(1).Returns("Sport 1");
            cacheRefService.GetBetTypeName(7).Returns("Bettype 7");
            cacheRefService.GetBetTypeName(8).Returns("Bettype 8");
            var events = BetChoiceMatchingTestCases.Events;
            var ips = BetChoiceMatchingTestCases.IPs;
            var queryResult = new Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceSharedIPModel>>(events, ips);
            betChoiceMatchingService.Query(Arg.Any<BetChoiceMatchingParam>()).Returns(queryResult);

            // Act
            var actual = handler.Handle(message);

            // Assert
            Assert.Equal(events, actual.Events);
            Assert.Equal(ips, actual.IPs);
            var expectedLeagues = new List<KeyValueModel>
            {
                new KeyValueModel(8, "League 8")
            };
            AssertExtension.Equal(expectedLeagues, actual.Leagues.ToList());
            var expectedTeams = new List<KeyValueModel>
            {
                new KeyValueModel(1, "Team 1"),
                new KeyValueModel(2, "Team 2")
            };
            AssertExtension.Equal(expectedTeams, actual.Teams.ToList());
            var expectedSportTypes = new List<KeyValueModel>
            {
                new KeyValueModel(1, "Sport 1")
            };
            AssertExtension.Equal(expectedSportTypes, actual.SportTypes.ToList());
            var expectedBetTypes = new List<KeyValueModel>
            {
                new KeyValueModel(7, "Bettype 7"),
                new KeyValueModel(8, "Bettype 8")
            };
            AssertExtension.Equal(expectedBetTypes, actual.BetTypes.ToList());
        }
    }
}
