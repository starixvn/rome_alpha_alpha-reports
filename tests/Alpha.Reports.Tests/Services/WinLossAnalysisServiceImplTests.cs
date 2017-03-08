namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossAnalysis;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinLossAnalysis;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    public class WinLossAnalysisServiceImplTests
    {
        private readonly WinLossAnalysisDataAccess dataAccess;
        private readonly WinLossAnalysisServiceImpl winLossAnalysisServiceImpl;
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly WinLossAnalysisParam param;

        public WinLossAnalysisServiceImplTests()
        {
            var productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossAnalysisDataAccess>();
            reportDateService = Substitute.For<ReportDateService>();
            winLossAnalysisServiceImpl = new WinLossAnalysisServiceImpl(reportDateService, productService, dataAccess);
            compareLogic = new CompareLogic();

            param = new WinLossAnalysisParam(0, 2, currentDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super);
        }

        [Fact]
        public void GetWinLossAnalysis_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossAnalysisParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossAnalysisServiceImpl.GetWinLossAnalysis(nullParam));
        }

        [Fact]
        public void GetWinLossAnalysis_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedData = Enumerable.Empty<WinLossAnalysisModel>();

            // Act
            dataAccess.GetWinLossAnalysis(Arg.Any<WinLossAnalysisCriteria>()).Returns(expectedData);
            var actualData = winLossAnalysisServiceImpl.GetWinLossAnalysis(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossWinLossAnalysis_CallGetWinLossWinLossAnalysisDataAccess()
        {
            // Arrange

            // Act
            winLossAnalysisServiceImpl.GetWinLossAnalysis(param);

            // Assert
            dataAccess.Received().GetWinLossAnalysis(Arg.Any<WinLossAnalysisCriteria>());
        }
    }
}