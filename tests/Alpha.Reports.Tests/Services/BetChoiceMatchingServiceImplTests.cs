namespace Alpha.Reports.Tests.Services
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Tests.Extensions;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using Alpha.Utilities.IP;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingServiceImplTests
    {
        private readonly IDynamicRepository repository;
        private readonly IPService ipService;
        private readonly IUserSession userSession;
        private readonly BetChoiceMatchingService service;
        private readonly CompareLogic compareLogic;
        private readonly DateTime winlostDate = DateTime.Parse("2019-09-09");
        private readonly DateTime moveBetsDay = DateTime.Parse("2019-08-08");

        public BetChoiceMatchingServiceImplTests()
        {
            compareLogic = new CompareLogic();
            repository = Substitute.For<IDynamicRepository>();
            userSession = Substitute.For<IUserSession>();
            ipService = Substitute.For<IPService>();
            service = new BetChoiceMatchingServiceImpl(repository, userSession, ipService);
        }

        private void MockFetchMultiple()
        {
            var tickets = BetChoiceMatchingTestCases.Tickets;
            var ips = BetChoiceMatchingTestCases.IPs;
            var tuple = new Tuple<IEnumerable<BetChoiceMatchingTicketModel>, IEnumerable<BetChoiceMatchingSharedIPModel>>(tickets, ips);
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

            AssertExtension.Equal(BetChoiceMatchingTestCases.Events, queryEvents);
            Assert.Equal(BetChoiceMatchingTestCases.IPs, queryIPs);
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
            Assert.Equal(expectedIPs, queryIPs);
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
    }
}
