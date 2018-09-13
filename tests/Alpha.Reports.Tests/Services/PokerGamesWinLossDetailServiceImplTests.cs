namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.PokerGamesWinLossDetail;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    public class PokerGamesWinLossDetailServiceImplTests
    {
        private readonly PokerGamesWinLossDetailDataAccess dataAccess;
        private readonly PokerGamesWinLossDetailServiceImpl winLossDetailServiceImpl;
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;

        public PokerGamesWinLossDetailServiceImplTests()
        {
            dataAccess = Substitute.For<PokerGamesWinLossDetailDataAccess>();
            reportDateService = Substitute.For<ReportDateService>();
            winLossDetailServiceImpl = new PokerGamesWinLossDetailServiceImpl(reportDateService, dataAccess);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossDetail_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            PokerGamesWinLossDetailParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailServiceImpl.GetWinLossDetail(nullParam));
        }

        [Fact]
        public void GetWinLossDetail_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            var param = new PokerGamesWinLossDetailParam(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.MinValue,
                UserLevel.Super,
                string.Empty,
                false,
                false);

            var reportData = new PagedReportData<PokerGamesWinLossDetailModel>(Enumerable.Empty<PokerGamesWinLossDetailModel>(), param.PageIndex, param.PageSize, 0, new PokerGamesWinLossDetailModel());
            dataAccess.GetData(Arg.Any<PokerGamesWinLossDetailCriteria>()).Returns(reportData);

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
            PokerGamesWinLossDetailParam param = new PokerGamesWinLossDetailParam(
                0,
                0,
                0,
                DateTime.Now,
                 DateTime.Today.AddDays(1),
                limitedDate,
                UserLevel.Super,
                string.Empty,
                false,
                false);

            // Act
            winLossDetailServiceImpl.GetWinLossDetail(param);

            // Assert
            dataAccess.Received().GetData(Arg.Is<PokerGamesWinLossDetailCriteria>(i => i.FromDate == limitedDate));
        }
    }
}