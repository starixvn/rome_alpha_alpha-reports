namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Linq;
    using Extensions;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossDetail;
    using Reports.DataAccesses;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailServiceImplTests
    {
        private readonly WinLossDetailDataAccess dataAccess;
        private readonly WinLossDetailServiceImpl winLossDetailServiceImpl;
        private readonly ReportDateService reportDateService;
        private readonly CompareLogic compareLogic;

        public WinLossDetailServiceImplTests()
        {
            reportDateService = Substitute.For<ReportDateService>();
            var productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossDetailDataAccess>();
            winLossDetailServiceImpl = new WinLossDetailServiceImpl(reportDateService, productService, dataAccess);
            reportDateService.ValidateDateRange(Arg.Any<DateRange>()).Returns(true);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossDetail_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailServiceImpl.GetWinLossDetail(nullParam));
        }

        [Fact]
        public void GetWinLossDetail_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossDetailParam param = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                string.Empty,
                false,
                false);

            var reportData = new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), param.PageIndex, param.PageSize, 0, new WinLossDetailModel());
            dataAccess.GetData(Arg.Any<WinLossDetailCriteria>()).Returns(reportData);

            var expectedReportData = reportData;

            // Act
            var actualReportData = winLossDetailServiceImpl.GetWinLossDetail(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedReportData, actualReportData).AreEqual);
        }

        [Fact]
        public void GetWinLossDetail_InvalidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossDetailParam param = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.Now.AddYears(-4),
                DateTime.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                string.Empty,
                false,
                false);
            var expectedReportData = new ReportData<WinLossDetailModel>(
                Enumerable.Empty<WinLossDetailModel>(),
                param.PageIndex,
                param.PageSize,
                0,
                new WinLossDetailModel());
            reportDateService.ValidateDateRange(Arg.Any<DateRange>()).Returns(false);

            // Act
            var actualReportData = winLossDetailServiceImpl.GetWinLossDetail(param);

            // Assert
            AssertExtension.Equal(expectedReportData, actualReportData);
        }
    }
}