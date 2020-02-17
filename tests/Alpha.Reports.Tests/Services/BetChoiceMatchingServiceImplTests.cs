namespace Alpha.Reports.Tests.Services
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Tests.Extensions;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingServiceImplTests
    {
        private readonly IDynamicRepository repository;
        private readonly BetChoiceMatchingService service;
        private readonly CompareLogic compareLogic;

        public BetChoiceMatchingServiceImplTests()
        {
            compareLogic = new CompareLogic();
            repository = Substitute.For<IDynamicRepository>();
            service = new BetChoiceMatchingServiceImpl(repository);
        }

        [Fact]
        public void Query_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var tickets = BetChoiceMatchingTestCases.Tickets;
            var ips = BetChoiceMatchingTestCases.IPs;
            var tuple = new Tuple<IEnumerable<BetChoiceMatchingTicketModel>, IEnumerable<BetChoiceSharedIPModel>>(tickets, ips);
            repository.FetchMultiple<BetChoiceMatchingTicketModel, BetChoiceSharedIPModel>(Arg.Any<BetChoiceMatchingCriteria>()).Returns(tuple);

            // Act
            var param = new BetChoiceMatchingParam(1, new DateTime(), new DateTime(), 2);
            var queryResult = service.Query(param);

            // Assert
            var queryEvents = queryResult.Item1;
            var queryIPs = queryResult.Item2;
            
            AssertExtension.Equal(BetChoiceMatchingTestCases.EventsJson, queryEvents);
            Assert.Equal(ips, queryIPs);
        }
    }
}
