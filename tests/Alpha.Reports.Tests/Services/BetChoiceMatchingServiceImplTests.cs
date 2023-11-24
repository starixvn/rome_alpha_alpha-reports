namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Tests.Extensions;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using AlphaPlus.PartialUM.Services;
    using AlphaPlus.User.Models;
    using AlphaPlus.Utilities.IP;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingServiceImplTests
    {
        private readonly IDynamicRepository repository;
        private readonly IPService ipService;
        private readonly IUserSession userSession;
        private readonly BetChoiceMatchingService service;
        private readonly CacheRefService cacheRefService;
        private readonly IPartialUMService partialUMService;
        private readonly DateTime winlostDate = DateTime.Parse("2019-09-09");
        private readonly DateTime moveBetsDay = DateTime.Parse("2019-08-08");

        public BetChoiceMatchingServiceImplTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            userSession = Substitute.For<IUserSession>();
            ipService = Substitute.For<IPService>();
            cacheRefService = Substitute.For<CacheRefService>();
            partialUMService = Substitute.For<IPartialUMService>();
            service = new BetChoiceMatchingServiceImpl(repository, userSession, ipService, cacheRefService, partialUMService);
        }

        private void MockFetchMultiple()
        {
            var tickets = BetChoiceMatchingTestCases.Tickets;
            var ips = BetChoiceMatchingTestCases.IPs;
            var tuple = (tickets, ips);
            cacheRefService.GetOddsTypeName(Arg.Any<string>()).Returns(callInfo =>
            {
                var oddsType = callInfo.Arg<string>();
                return $"OddsType{oddsType}";
            });
            repository.FetchMultiple<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(Arg.Any<BetChoiceMatchingCriteria>()).Returns(tuple);
        }

        [Fact]
        public void QueryWithCanViewVNIP_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            userSession.IsAdminCanViewVNIP.Returns(true);
            ipService.IsVNIP(Arg.Any<string>()).Returns(callInfo =>
            {
                var ip = callInfo.Arg<string>();
                return ip == "127.0.0.1";
            });
            var param = new BetChoiceMatchingParam(1, winlostDate, moveBetsDay, 2);

            // Act
            var queryResult = service.Query(param);

            // Assert
            var queryEvents = queryResult.Item1;
            var queryIPs = queryResult.Item2;
            var expectedEvents = BetChoiceMatchingTestCases.Events;
            var expectedIPs = BetChoiceMatchingTestCases.IPs;
            AssertExtension.Equal(expectedEvents, queryEvents);
            AssertExtension.Equal(expectedIPs, queryIPs);
        }

        [Fact]
        public void QueryWithCanNotViewVNIP_Always_ReturnsDataFromDynamicRepositoryFiltered()
        {
            // Arrange
            MockFetchMultiple();
            userSession.IsAdminCanViewVNIP.Returns(false);
            ipService.IsVNIP(Arg.Any<string>()).Returns(callInfo =>
            {
                var ip = callInfo.Arg<string>();
                return ip == "127.0.0.1";
            });
            var param = new BetChoiceMatchingParam(1, winlostDate, moveBetsDay, 2);

            // Act
            var queryResult = service.Query(param);

            // Assert
            var queryEvents = queryResult.Item1;
            var queryIPs = queryResult.Item2;
            var expectedEvents = BetChoiceMatchingTestCases.Events;
            var event0User0Tickets = expectedEvents[0].Users.First().Tickets as List<BetChoiceMatchingTicketModel>;
            event0User0Tickets[0].HideIP();
            event0User0Tickets[1].HideIP();
            var expectedIPs = new List<BetChoiceMatchingSharedIPModel>
            {
                BetChoiceMatchingTestCases.IPs[2]
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
            AssertExtension.Equal(expectedIPs, queryIPs);
        }

        [Fact]
        public void QueryUplines_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var param = new BetChoiceUplinesParam(1);
            var uplinesModel = new BetChoiceMatchingUplinesModel(1, "Agent 1", 2, "Master 2");
            repository.Get<BetChoiceMatchingUplinesModel>(Arg.Any<BetChoiceUplinesCriteria>()).Returns(uplinesModel);

            // Act
            var queryResult = service.QueryUplines(param);

            // Assert
            AssertExtension.Equal(uplinesModel, queryResult);
        }

        [Fact]
        public void DefaultCtor_Always_ReturnsCorrectProperties()
        {
            // Arrange
            var model = new BetChoiceMatchingUplinesModel();

            // Assert
            Assert.True(model.AgentId == 0);

            // Assert
            Assert.True(model.MasterId == 0);

            // Assert
            Assert.Null(model.AgentName);

            // Assert
            Assert.Null(model.MasterName);
        }

        [Fact]
        public void ExcelQuery_WithNoFilter_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, null, null, MemberWinLossOperators.Equal, null);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            AssertExtension.Equal(BetChoiceMatchingTestCases.Events, queryEvents);
        }

        [Fact]
        public void ExcelQuery_WithUsernameFilter_ReturnsMatchingDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, "NK1", null, MemberWinLossOperators.Equal, null);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            var user = BetChoiceMatchingTestCases.Events.First().Users.First();
            var expectedEvents = new List<BetChoiceMatchingEventModel>
            {
                new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel>{user})
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
        }

        [Fact]
        public void ExcelQuery_WithLowerUsernameFilter_ReturnsMatchingDataFromDynamicRepositoryAndIgnoreCase()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, "nk1", null, MemberWinLossOperators.Equal, null);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            var user = BetChoiceMatchingTestCases.Events.First().Users.First();
            var expectedEvents = new List<BetChoiceMatchingEventModel>
            {
                new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel>{user})
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
        }

        [Fact]
        public void ExcelQuery_WithStakeFilter_ReturnsMatchingDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, null, 20, MemberWinLossOperators.Equal, null);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            var user = BetChoiceMatchingTestCases.Events.First().Users.First();
            var expectedEvents = new List<BetChoiceMatchingEventModel>
            {
                new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel>{user})
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
        }

        [Fact]
        public void ExcelQuery_WithEqualMemberWinLossFilter_ReturnsMatchingDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, null, null, MemberWinLossOperators.Equal, 4);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            var expectedEvents = new List<BetChoiceMatchingEventModel>
            {
                new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel>
                {
                    BetChoiceMatchingTestCases.Events[0].Users.ToList()[1]
                })
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
        }

        [Fact]
        public void ExcelQuery_WithGreaterMemberWinLossFilter_ReturnsMatchingDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, null, null, MemberWinLossOperators.GreatorOrEqual, 0m);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            var expectedEvents = new List<BetChoiceMatchingEventModel>
            {
                new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel>
                {
                    BetChoiceMatchingTestCases.Events[0].Users.ToList()[0],
                    BetChoiceMatchingTestCases.Events[0].Users.ToList()[1]
                })
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
        }

        [Fact]
        public void ExcelQuery_WithLessMemberWinLossFilter_ReturnsMatchingDataFromDynamicRepository()
        {
            // Arrange
            MockFetchMultiple();
            var param = new BetChoiceMatchingExcelParam(1, winlostDate, moveBetsDay, 2, null, null, MemberWinLossOperators.LessOrEqual, 3.5m);

            // Act
            var queryEvents = service.ExcelQuery(param);

            // Assert
            var expectedEvents = new List<BetChoiceMatchingEventModel>
            {
                new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel>
                {
                    BetChoiceMatchingTestCases.Events[0].Users.ToList()[0]
                })
            };

            AssertExtension.Equal(expectedEvents, queryEvents);
        }

        [Fact]
        public void BetChoice_DefaultCtor_AlwaysNull()
        {
            var act = new BetChoiceMatchingSharedIPModel();
            Assert.Null(act.IP);
            Assert.True(act.CustId == 0);
            Assert.Null(act.Username);
        }

        [Fact]
        public void BetChoice_ParamsCtor_AlwaysNotNull()
        {
            var act = new BetChoiceMatchingSharedIPModel("192.168.1.100", 1, "xyz");
            Assert.NotNull(act.IP);
            Assert.True(act.CustId >= 0);
            Assert.NotNull(act.Username);
        }
    }
}