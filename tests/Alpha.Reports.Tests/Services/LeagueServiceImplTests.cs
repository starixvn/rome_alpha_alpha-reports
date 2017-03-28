namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria;
    using Reports.Models;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("AnyValueCreatorCollection")]
    public class LeagueServiceImplTests
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly CacheRefService cacheRefService;

        private readonly LeagueServiceImpl leagueServiceImpl;

        public LeagueServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
            reportDateService = Substitute.For<ReportDateService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheRefService = Substitute.For<CacheRefService>();
            leagueServiceImpl = new LeagueServiceImpl(reportDateService, dynamicRepository, cacheRefService);
        }

        [Fact]
        public void GetLeaguesByCustomer_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            GetLeaguesByCustomerParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => leagueServiceImpl.GetLeaguesByCustomer(nullParam));
        }

        [Fact]
        public void GetLeaguesByCustomer_NotNullParam_CallsDynamicRepository()
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<string>());

            var moveBetsDay = anyValueCreatorFixture.Create<DateTime>();
            reportDateService.GetMoveBetsDay().Returns(moveBetsDay);

            // Act
            leagueServiceImpl.GetLeaguesByCustomer(param);

            // Assert
            dynamicRepository.Received().Fetch<League>(Arg.Is<GetLeaguesByCustomerCriteria>(
                criteria => criteria.CustId == param.CustId
                && criteria.ProductIds == param.ProductIds
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.Language == param.Language
                && criteria.MoveBetsDay == moveBetsDay));
        }

        [Fact]
        public void GetLeaguesByCustomer_NotNullParam_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<string>());

            var moveBetsDay = anyValueCreatorFixture.Create<DateTime>();
            reportDateService.GetMoveBetsDay().Returns(moveBetsDay);

            var expectedLeagues = new List<League>
            {
                new League(1, 1, "League 1"),
                new League(2, 2, "League 2")
            };

            dynamicRepository.Fetch<League>(Arg.Any<GetLeaguesByCustomerCriteria>()).Returns(expectedLeagues);

            // Act
            var actualLeagues = leagueServiceImpl.GetLeaguesByCustomer(param);

            // Assert
            AssertExtension.Equal(expectedLeagues, actualLeagues);
        }

        [Fact]
        public void GetLeagueNames_NullLeagueIds_ReturnsEmptyNameList()
        {
            // Arrange
            IEnumerable<int> leagueIds = null;

            // Act
            var actualLeagueNames = leagueServiceImpl.GetLeagueNames(leagueIds);

            // Assert
            Assert.Empty(actualLeagueNames);
        }

        [Fact]
        public void GetLeagueNames_NotNullLeagueIds_ReturnsCorrectNameList()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, 2, 3 };
            var expectedLeagueNames = new Dictionary<int, string>
            {
                [1] = "League 1",
                [2] = "League 2",
                [3] = "League 3"
            };

            cacheRefService.GetLeagueName(Arg.Any<int>()).Returns(callInfo => "League " + callInfo.Args()[0]);

            // Act
            var actualLeagueNames = leagueServiceImpl.GetLeagueNames(productIds);

            // Assert
            Assert.Equal(expectedLeagueNames, actualLeagueNames);
        }
    }
}
