namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Constants;
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
                new League(1, 1, "League 1", 0),
                new League(2, 2, "League 2", 0)
            };

            dynamicRepository.Fetch<League>(Arg.Any<GetLeaguesByCustomerCriteria>()).Returns(expectedLeagues);

            // Act
            var actualLeagues = leagueServiceImpl.GetLeaguesByCustomer(param);

            // Assert
            AssertExtension.Equal(expectedLeagues, actualLeagues);
        }

        [Fact]
        public void GetLeagueNames_InvalidLeagueId_ReturnsEmptyName()
        {
            // Arrange
            var leagueId = 0;

            // Act
            var actualLeagueName = leagueServiceImpl.GetLeagueName(leagueId, (byte)LeagueType.Self);

            // Assert
            Assert.Empty(actualLeagueName);
        }

        [Fact]
        public void GetLeagueNames_ValidLeagueId_ReturnsCorrectName()
        {
            // Arrange
            var productId = 1;
            var expectedLeagueName = "League 1";

            cacheRefService.GetLeagueName(Arg.Any<int>()).Returns(callInfo => "League " + callInfo.Args()[0]);

            // Act
            var actualLeagueName = leagueServiceImpl.GetLeagueName(productId, (byte)LeagueType.Self);

            // Assert
            Assert.Equal(expectedLeagueName, actualLeagueName);
        }
    }
}
