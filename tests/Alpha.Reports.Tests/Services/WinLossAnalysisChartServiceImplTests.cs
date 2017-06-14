namespace Alpha.Reports.Tests.Services
{
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossAnalysisChart;
    using Reports.Models;
    using Reports.Models.WinLossAnalysisChart;
    using Reports.Params;
    using Reports.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    public class WinLossAnalysisChartServiceImplTests
    {
        private readonly WinLossAnalysisChartServiceImpl winLossAnalysisChartServiceImpl;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;
        private readonly WinLossAnalysisChartParam param;

        public WinLossAnalysisChartServiceImplTests()
        {
            var productService = Substitute.For<ProductService>();
            reportDateService = Substitute.For<ReportDateService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            winLossAnalysisChartServiceImpl = new WinLossAnalysisChartServiceImpl(reportDateService, productService, dynamicRepository);
            compareLogic = new CompareLogic();

            param = new WinLossAnalysisChartParam(0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), UserLevel.Super);
        }

        [Fact]
        public void GetWinLossAnalysisChart_ValidParam_ReturnDefaultReportData()
        {
            // Arrange
            var expectedData = new List<WinLossAnalysisChartModel>
            {
                new WinLossAnalysisChartModel("2017-01", 12051.12m, 56300.16m)
            };

            dynamicRepository.Fetch<WinLossAnalysisChartModel>(Arg.Any<WinLossAnalysisChartCriteria>()).Returns(expectedData);

            // Act
            var actualData = winLossAnalysisChartServiceImpl.GetWinLossAnalysisChart(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);

            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossAnalysisChart_ValidParam_CallsDynamicRepository()
        {
            // Act
            winLossAnalysisChartServiceImpl.GetWinLossAnalysisChart(param);

            // Assert
            dynamicRepository.Received().Fetch<WinLossAnalysisChartModel>(Arg.Any<WinLossAnalysisChartCriteria>());
        }
    }
}