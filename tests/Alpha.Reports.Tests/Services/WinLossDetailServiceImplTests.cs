namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Linq;
    using Extensions;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossDetail;
    using Reports.DataAccesses;
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
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;

        public WinLossDetailServiceImplTests()
        {
            var productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossDetailDataAccess>();
            reportDateService = Substitute.For<ReportDateService>();
            winLossDetailServiceImpl = new WinLossDetailServiceImpl(reportDateService, productService, dataAccess);
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
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                Enumerable.Empty<int>());

            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), param.PageIndex, param.PageSize, 0, new WinLossDetailModel());
            dataAccess.GetData(Arg.Any<WinLossDetailCriteria>()).Returns(reportData);

            var expectedReportData = reportData;

            // Act
            var actualReportData = winLossDetailServiceImpl.GetWinLossDetail(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedReportData, actualReportData).AreEqual);
        }

        [Fact]
        public void GetWinLossDetail_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = DateTime.Today.AddDays(1);
            WinLossDetailParam param = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.Now,
                 DateTime.Today.AddDays(1),
                limitedDate,
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                Enumerable.Empty<int>());            

            // Act
            winLossDetailServiceImpl.GetWinLossDetail(param);

            // Assert
            dataAccess.Received().GetData(Arg.Is<WinLossDetailCriteria>(i => i.FromDate == limitedDate));
        }
    }
}