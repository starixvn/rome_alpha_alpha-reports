namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria.ProgressiveContribution;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.ProgressiveContribution;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    [Collection("ObjectDbCollection")]
    public class ProgressiveContributionDataAccessTests
    {
        private readonly CompareLogic compareLogic;
        private readonly ObjectDbFixture objectDbFixture;
        private readonly CacheRefService cacheRefService;
        private readonly ProgressiveContributionDataAccessImpl progressiveContributionDataAccess;

        public ProgressiveContributionDataAccessTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            compareLogic = new CompareLogic();

            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetBetTypeName(Arg.Is(2179)).Returns("Baccarat (Progressive JP) - Jackpot Game");
            cacheRefService.GetBetTypeName(2320).Returns("(Mobile) Baccarat (Progressive JP) - Jackpot Game");
            cacheRefService.GetBetTypeName(2920).Returns("(Tablet) Baccarat (Progressive JP) - Jackpot Game");

            progressiveContributionDataAccess = new ProgressiveContributionDataAccessImpl(cacheRefService);
        }

        [Fact]
        public void GetProgressiveContributions_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ProgressiveContributionCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => progressiveContributionDataAccess.GetProgressiveContributions(nullCriteria));
        }

        [Fact]
        public void GetProgressiveContributions_AgentLevel_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new ProgressiveContributionCriteria(1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, new List<int> { 6 }, UserLevel.Agent);

            var dataset = CreateSampleData().CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<ProgressiveContributionCriteria>()).Returns(dataset);

            var expectedData = new List<ProgressiveContributionByBetTypeModel>
            {
                new ProgressiveContributionByBetTypeModel(6,2179,"Baccarat (Progressive JP) - Jackpot Game",2000m,100m,-40m,0,0),
                new ProgressiveContributionByBetTypeModel(6,2320,"(Mobile) Baccarat (Progressive JP) - Jackpot Game",3000m,150m,-60m,0,0),
                new ProgressiveContributionByBetTypeModel(6,2920,"(Tablet) Baccarat (Progressive JP) - Jackpot Game",5000m,250m,-100m,0,0)
            };

            // Act
            var actualData = progressiveContributionDataAccess.GetProgressiveContributions(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetProgressiveContributions_MasterLevel_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new ProgressiveContributionCriteria(1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, new List<int> { 6 }, UserLevel.Master);

            var dataset = CreateSampleData().CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<ProgressiveContributionCriteria>()).Returns(dataset);

            var expectedData = new List<ProgressiveContributionByBetTypeModel>
            {
                new ProgressiveContributionByBetTypeModel(6,2179,"Baccarat (Progressive JP) - Jackpot Game",2000m,100m,-40m,-30m,0),
                new ProgressiveContributionByBetTypeModel(6,2320,"(Mobile) Baccarat (Progressive JP) - Jackpot Game",3000m,150m,-60m,-45m,0),
                new ProgressiveContributionByBetTypeModel(6,2920,"(Tablet) Baccarat (Progressive JP) - Jackpot Game",5000m,250m,-100m,-75m,0)
            };

            // Act
            var actualData = progressiveContributionDataAccess.GetProgressiveContributions(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetProgressiveContributions_SuperLevel_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new ProgressiveContributionCriteria(1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, new List<int> { 6 }, UserLevel.Super);

            var dataset = CreateSampleData().CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<ProgressiveContributionCriteria>()).Returns(dataset);

            var expectedData = new List<ProgressiveContributionByBetTypeModel>
            {
                new ProgressiveContributionByBetTypeModel(6,2179,"Baccarat (Progressive JP) - Jackpot Game",2000m,100m,-40m,-30m,-20m),
                new ProgressiveContributionByBetTypeModel(6,2320,"(Mobile) Baccarat (Progressive JP) - Jackpot Game",3000m,150m,-60m,-45m,-30m),
                new ProgressiveContributionByBetTypeModel(6,2920,"(Tablet) Baccarat (Progressive JP) - Jackpot Game",5000m,250m,-100m,-75m,-50m)
            };

            // Act
            var actualData = progressiveContributionDataAccess.GetProgressiveContributions(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        private static DataTable CreateSampleData()
        {
            var table = new DataTable();
            table.Columns.Add("ProductId");
            table.Columns.Add("BetType");
            table.Columns.Add("TurnOver");
            table.Columns.Add("Contribution");
            table.Columns.Add("ContributionAgent");
            table.Columns.Add("ContributionMaster");
            table.Columns.Add("ContributionSuper");

            table.Rows.Add(6, 2179, 2000m, 100m, -40m, -30m, -20m);
            table.Rows.Add(6, 2320, 3000m, 150m, -60m, -45m, -30m);
            table.Rows.Add(6, 2920, 5000m, 250m, -100m, -75m, -50m);

            return table;
        }
    }
}