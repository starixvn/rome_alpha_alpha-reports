namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.ProgressiveContribution;
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
        private readonly IDynamicRepository repository;
        private readonly IPartialUMService partialUMService;
        private readonly ProgressiveContributionServiceImpl progressiveContributionService;

        public ProgressiveContributionServiceImplTests()
        {
            compareLogic = new CompareLogic();

            reportDateService = Substitute.For<ReportDateService>();
            cacheRefService = Substitute.For<CacheRefService>();
            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<IPartialUMService>();

            progressiveContributionService = new ProgressiveContributionServiceImpl(
                reportDateService,
                cacheRefService,
                partialUMService,
                repository);
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
            repository.Fetch<ProgressiveContributionByBetTypeModel>(Arg.Any<ProgressiveContributionCriteria>()).Returns(progressiveContributions);

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

            repository.Fetch<ProgressiveContributionByBetTypeModel>(Arg.Any<ProgressiveContributionCriteria>()).Returns(progressiveContributions);
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
            repository
                .Received()
                .Fetch<ProgressiveContributionByBetTypeModel>(Arg.Is<ProgressiveContributionCriteria>(i => i.FromDate == limitedDate));
        }
    }
}