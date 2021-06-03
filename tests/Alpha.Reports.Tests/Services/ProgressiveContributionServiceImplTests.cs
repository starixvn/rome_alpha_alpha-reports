namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.ProgressiveContribution;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.ProgressiveContribution;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    public class ProgressiveContributionServiceImplTests
    {
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;
        private readonly ProgressiveContributionDataAccess progressiveContributionDataAccess;
        private readonly PartialUMService partialUMService;
        private readonly ProgressiveContributionServiceImpl progressiveContributionService;

        public ProgressiveContributionServiceImplTests()
        {
            compareLogic = new CompareLogic();

            reportDateService = Substitute.For<ReportDateService>();
            cacheRefService = Substitute.For<CacheRefService>();
            progressiveContributionDataAccess = Substitute.For<ProgressiveContributionDataAccess>();
            partialUMService = Substitute.For<PartialUMService>();

            progressiveContributionService = new ProgressiveContributionServiceImpl(
                progressiveContributionDataAccess,
                reportDateService,
                cacheRefService,
                partialUMService);
        }

        [Fact]
        public void GetProgressiveContributions_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            ProgressiveContributionParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => progressiveContributionService.GetProgressiveContributions(nullParam));
        }

        [Fact]
        public void GetProgressiveContributions_EmptyData_ReturnsDefaultReportData()
        {
            // Arrange
            var param = new ProgressiveContributionParam(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                UserLevel.Super);

            var progressiveContributions = Enumerable.Empty<ProgressiveContributionByBetTypeModel>();
            progressiveContributionDataAccess.GetProgressiveContributions(Arg.Any<ProgressiveContributionCriteria>()).Returns(progressiveContributions);

            var expectedData = new List<ProgressiveContributionByProductModel>();

            // Act
            var actualData = progressiveContributionService.GetProgressiveContributions(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetProgressiveContributions_HasData_ReturnsCorrectModel()
        {
            // Arrange
            var param = new ProgressiveContributionParam(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                UserLevel.Super);

            var progressiveContributions =
                new List<ProgressiveContributionByBetTypeModel> {
                    new ProgressiveContributionByBetTypeModel(6, 2179, string.Empty, 2000, 100, -40, -30, -20),
                    new ProgressiveContributionByBetTypeModel(6, 2320, string.Empty, 3000, 150, -60, -45, -30),
                    new ProgressiveContributionByBetTypeModel(6, 2320, string.Empty, 5000, 550, -100, -75, -50) };

            progressiveContributionDataAccess.GetProgressiveContributions(Arg.Any<ProgressiveContributionCriteria>()).Returns(progressiveContributions);
            var expectedData = new List<ProgressiveContributionByProductModel>
            {
                new ProgressiveContributionByProductModel(6,string.Empty,progressiveContributions)
            };

            // Act
            var actualData = progressiveContributionService.GetProgressiveContributions(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetProgressiveContributions_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = DateTime.Today.AddDays(1);
            var param = new ProgressiveContributionParam(
                0,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                limitedDate,
                Enumerable.Empty<int>(),
                UserLevel.Super);  

            // Act
            progressiveContributionService.GetProgressiveContributions(param);

            // Assert
            progressiveContributionDataAccess
                .Received()
                .GetProgressiveContributions(Arg.Is<ProgressiveContributionCriteria>(i => i.FromDate == limitedDate));
        }
    }
}