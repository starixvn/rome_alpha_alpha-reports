namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossAnalysisChart;
    using Reports.Models;
    using Reports.Models.WinLossAnalysisChart;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    public class WinLossAnalysisChartServiceImplTests
    {
        private readonly WinLossAnalysisChartServiceImpl winLossAnalysisChartServiceImpl;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ProductService productService;
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;
        private readonly WinLossAnalysisChartParam param;
        private readonly List<int> ProductIds;
        private readonly IPartialUMService partialUMService;

        public WinLossAnalysisChartServiceImplTests()
        {
            ProductIds = new List<int> { 1, 2 };
            productService = Substitute.For<ProductService>();
            reportDateService = Substitute.For<ReportDateService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<IPartialUMService>();
            winLossAnalysisChartServiceImpl = new WinLossAnalysisChartServiceImpl(reportDateService, productService, dynamicRepository, partialUMService);
            compareLogic = new CompareLogic();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(ProductIds);
            param = new WinLossAnalysisChartParam(1, 1, "", 1, DateTime.Now.AddMonths(-3), DateTime.Now, DateTime.MinValue, ProductIds, UserLevel.Super);
        }

        [Fact]
        public void GetWinLossAnalysisChart_ValidParam_ReturnDefaultReportData()
        {
            // Arrange
            var expectedData = new List<WinLossAnalysisChartModel>
            {
                new WinLossAnalysisChartModel(DateTime.Today.AddMonths(-3).ToString("yyyy-MM"), 12051.12m, 56300.16m),
                new WinLossAnalysisChartModel(DateTime.Today.AddMonths(-2).ToString("yyyy-MM"), 12051.12m, 56300.16m),
                new WinLossAnalysisChartModel(DateTime.Today.AddMonths(-1).ToString("yyyy-MM"), 12051.12m, 56300.16m),
                new WinLossAnalysisChartModel(DateTime.Today.ToString("yyyy-MM"), 12051.12m, 56300.16m)
            };
            dynamicRepository.Fetch<WinLossAnalysisChartModel>(Arg.Any<WinLossAnalysisChartCriteria>()).Returns(expectedData);

            // Act
            var actualData = winLossAnalysisChartServiceImpl.GetWinLossAnalysisChart(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);

            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossAnalysisChart_DefaultCtor_ReturnDefaultReportData()
        {
            // Arrange
            var model = new WinLossAnalysisChartModel();

            // Assert
            Assert.Null(model.WinLossDate);

            // Assert
            Assert.True(model.Turnover == 0m);

            // Assert
            Assert.True(model.WinLoss == 0m);
        }

        [Fact]
        public void GetWinLossAnalysisChart_ValidParam_CallsDynamicRepository()
        {
            // Act
            winLossAnalysisChartServiceImpl.GetWinLossAnalysisChart(param);

            // Assert
            dynamicRepository.Received().Fetch<WinLossAnalysisChartModel>(Arg.Any<WinLossAnalysisChartCriteria>());
        }

        [Fact]
        public void GetWinLossAnalysisChart_UserIsInLimitedDateRange_ResetWinlossInLimitedDate()
        {
            // Arrange
            var limitedDate = new DateTime(2017, 3, 29);
            var param = new WinLossAnalysisChartParam(1, 1, "", 1, new DateTime(2017, 2, 1), new DateTime(2017, 5, 1), limitedDate, new List<int> { 1, 2 }, UserLevel.Super);
            var dataFromSp = new List<WinLossAnalysisChartModel>
            {
                new WinLossAnalysisChartModel("2017-03", 13051.12m, 56300.13m),
                new WinLossAnalysisChartModel("2017-04", 14051.12m, 56300.14m),
                new WinLossAnalysisChartModel("2017-05", 15051.12m, 56300.15m)
            };
            var expectedWinloss = new List<WinLossAnalysisChartModel>
            {
                new WinLossAnalysisChartModel("2017-02", 0, 0),
                new WinLossAnalysisChartModel("2017-03", 13051.12m, 56300.13m),
                new WinLossAnalysisChartModel("2017-04", 14051.12m, 56300.14m),
                new WinLossAnalysisChartModel("2017-05", 15051.12m, 56300.15m)
            };

            dynamicRepository.Fetch<WinLossAnalysisChartModel>(Arg.Any<WinLossAnalysisChartCriteria>()).Returns(dataFromSp);

            // Act
            var actualData = winLossAnalysisChartServiceImpl.GetWinLossAnalysisChart(param);

            // Assert
            var result = compareLogic.Compare(expectedWinloss, actualData);

            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossAnalysisChart_MonthRangeIsLessThan3Months_ReturnEmpty()
        {
            // Arrange
            var param = new WinLossAnalysisChartParam(1, 1, "", 1, DateTime.Now, DateTime.Now, DateTime.MinValue, ProductIds, UserLevel.Super);

            // Act
            var actualData = winLossAnalysisChartServiceImpl.GetWinLossAnalysisChart(param);

            // Assert
            Assert.Equal(0, actualData.Count());
        }
    }
}